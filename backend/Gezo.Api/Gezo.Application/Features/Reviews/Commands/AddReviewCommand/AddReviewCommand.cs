using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Reviews.Commands.AddReviewCommand
{
    public record AddReviewCommand : IRequest<ApiResponse<bool>>
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<bool>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GenericRepository<Product>().GetbyIdAsync(request.ProductId);
            if (product is null)
                throw new NotFoundCustomException("Product not found");

            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new BadRequestCustomException("User not found");

            var review = new Review
            {
                ProductId = request.ProductId,
                UserId = userId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GenericRepository<Review>().AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Review added successfully", StatusCodes.Status201Created);
        }
    }
}

