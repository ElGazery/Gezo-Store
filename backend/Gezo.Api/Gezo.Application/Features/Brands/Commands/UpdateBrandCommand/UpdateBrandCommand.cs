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

namespace Gezo.Application.Features.Brands.Commands.UpdateBrandCommand
{
    public record UpdateBrandCommand : IRequest<ApiResponse<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public IFormFile? Logo { get; set; }
    }

    internal class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public UpdateBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ApiResponse<int>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _unitOfWork.GenericRepository<Brand>().GetbyIdAsync(request.Id);
            if (brand is null)
                throw new NotFoundCustomException("Brand not found");

            _mapper.Map(request, brand);

            if (request.Logo is not null)
            {
                var logoUrl = await _imageService.UploadAsync(request.Logo);
                brand.LogoUrl = logoUrl;
            }

            brand.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>(brand.Id, "Brand updated successfully", StatusCodes.Status200OK);
        }
    }
}

