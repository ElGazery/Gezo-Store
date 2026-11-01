using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Products.Commands.DeleteProductCommand
{
    public record DeleteProductCommand(int id) :IRequest<ApiResponse<int>>
    {

    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<int>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GenericRepository<Product>().GetbyIdAsync(request.id);
            if (product is null)
                throw new BadRequestCustomException("Product not found");
            product.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>(product.Id, "Product deleted successfully", StatusCodes.Status200OK);
        }
    }

}
