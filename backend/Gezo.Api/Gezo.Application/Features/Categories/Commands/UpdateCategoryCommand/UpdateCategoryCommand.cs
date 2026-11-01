using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Commands.UpdateCategoryCommand
{
    public record UpdateCategoryCommand:IRequest<ApiResponse<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }

    internal class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork,IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<ApiResponse<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.GenericRepository<Category>().GetbyIdAsync(request.Id);
            if (category is null)
                throw new NotFoundCustomException("Category not found ");
            _mapper.Map(request, category);
            if (request.ImageUrl is not null)
            {
                var imageUrl = await _imageService.UploadAsync(request.ImageUrl);
                category.ImageUrl = imageUrl;
            }
            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>(category.Id, "Category Updated Successfuly", StatusCodes.Status200OK);
        }
    }
}
