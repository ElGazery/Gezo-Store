using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gezo.Application.Features.Products.Commands.AddProductCommand
{
    public record AddProductCommand :IRequest<ApiResponse<int>>
    {
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

        public List<IFormFile> Images { get; set; }
    }
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ApiResponse<int>>
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        public AddProductCommandHandler(IMapper mapper, IImageService imageService,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<int>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            var productImages = new List<ProductImage>();

            for (int i = 0; i < request.Images.Count; i++)
            {
                var imageUrl = await _imageService.UploadAsync(request.Images[i]);

                productImages.Add(new ProductImage
                {
                    ImageUrl = imageUrl,
                    DisplayOrder = i,
                    IsPrimary = i == 0
                });
            }
            product.Images = productImages;

            await _unitOfWork.GenericRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>(product.Id, "Product added successfuly", StatusCodes.Status201Created);
        }
    }
}
