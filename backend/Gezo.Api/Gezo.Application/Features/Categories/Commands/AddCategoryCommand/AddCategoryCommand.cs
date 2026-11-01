using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Commands.AddCategoryCommand
{
    public record AddCategoryCommand:IRequest<ApiResponse<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsActive { get; set; }
    }
    internal class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<ApiResponse<int>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            if (request.Image != null)
            {
                var imageUrl = await _imageService.UploadAsync(request.Image);
                category.ImageUrl = imageUrl;
            }
            category.IsActive = true;
            await _unitOfWork.GenericRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>(category.Id, "Categoy added successfuly", StatusCodes.Status201Created);
        }
    }
}
