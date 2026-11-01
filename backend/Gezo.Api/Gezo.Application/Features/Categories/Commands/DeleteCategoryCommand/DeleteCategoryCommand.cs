using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Commands.DeleteCategoryCommand
{
    public record DeleteCategoryCommand(int id):IRequest<ApiResponse<int>>
    {
    }
    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var catergory = await _unitOfWork.GenericRepository<Category>().GetbyIdAsync(request.id);
            if (catergory is null)
                throw new NotFoundCustomException("Category is not found");
            catergory.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<int>(catergory.Id, "Category Deleted successfuly", StatusCodes.Status200OK);
        }
    }
}
