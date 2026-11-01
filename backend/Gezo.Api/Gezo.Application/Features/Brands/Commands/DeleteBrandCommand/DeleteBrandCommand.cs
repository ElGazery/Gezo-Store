using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Brands.Commands.DeleteBrandCommand
{
    public record DeleteBrandCommand(int id) : IRequest<ApiResponse<int>>
    {
    }

    internal class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBrandCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<int>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _unitOfWork.GenericRepository<Brand>().GetbyIdAsync(request.id);
            if (brand is null)
                throw new NotFoundCustomException("Brand not found");

            brand.IsActive = false;
            brand.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>(brand.Id, "Brand deleted successfully", StatusCodes.Status200OK);
        }
    }
}

