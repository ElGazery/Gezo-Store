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

namespace Gezo.Application.Features.Brands.Commands.AddBrandCommand
{
    public record AddBrandCommand : IRequest<ApiResponse<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public IFormFile? Logo { get; set; }
        public bool IsActive { get; set; } = true;
    }

    internal class AddBrandCommandHandler : IRequestHandler<AddBrandCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public AddBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ApiResponse<int>> Handle(AddBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = _mapper.Map<Brand>(request);
            if (request.Logo != null)
            {
                var logoUrl = await _imageService.UploadAsync(request.Logo);
                brand.LogoUrl = logoUrl;
            }
            brand.IsActive = true;
            brand.CreatedAt = DateTime.UtcNow;
            brand.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.GenericRepository<Brand>().AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<int>(brand.Id, "Brand added successfully", StatusCodes.Status201Created);
        }
    }
}

