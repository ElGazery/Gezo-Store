
using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gezo.Application.Features.Products.Commands.UpdateProductCommand
{
    public record UpdateProductCommand : IRequest<ApiResponse<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public string SKU { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }

        // New images to upload (optional)
        public List<IFormFile>? NewImages { get; set; } = [];

        // Delete images
        public List<int>? DeletedImageIds { get; set; } = [];
    }

    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public UpdateProductCommandHandler( IUnitOfWork unitOfWork, IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ApiResponse<int>> Handle( UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork
                .GenericRepository<Product>()
                .GetbyIdAsync(request.Id);

            if (product == null)
                throw new BadRequestCustomException("Product Not Found");

            var imagesToDelete = new List<string>();

            var nextDisplayOrder = product.Images.Count > 0
                ? product.Images.Max(x => x.DisplayOrder) + 1
                : 0;

            if (request.NewImages?.Any() == true)
            {
                for (int i = 0; i < request.NewImages.Count; i++)
                {
                    var imageUrl = await _imageService.UploadAsync(
                        request.NewImages[i]);

                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imageUrl,
                        DisplayOrder = nextDisplayOrder++
                    });
                }
            }

            if (request.DeletedImageIds?.Any() == true)
            {
                for (int i = 0; i < request.DeletedImageIds.Count; i++)
                {
                    var image = await _unitOfWork.GenericRepository<ProductImage>().GetbyIdAsync(request.DeletedImageIds[i]);

                    if (image == null || image.ProductId != product.Id)
                        throw new BadRequestCustomException("Invalid Image");

                    imagesToDelete.Add(image.ImageUrl);

                    await _unitOfWork.GenericRepository<ProductImage>().DeleteAsync(image);
                }
            }

            _mapper.Map(request, product);

            await _unitOfWork.SaveChangesAsync();

            foreach (var imagePath in imagesToDelete)
            {
                await _imageService.DeleteAsync(imagePath);
            }

            return new ApiResponse<int>(  product.Id, "Product Updated Successfully",  StatusCodes.Status200OK);
        }
    }
}
