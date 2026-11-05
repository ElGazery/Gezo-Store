using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Application.Specifications.Wishlist;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Wishlist.Commands.AddToWishlistCommand
{
    public record AddToWishlistCommand(int ProductId) : IRequest<ApiResponse<bool>>
    {
    }

    public class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddToWishlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<bool>> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GenericRepository<Product>().GetbyIdAsync(request.ProductId);
            if (product is null)
                throw new NotFoundCustomException("Product not found");

            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new BadRequestCustomException("User not found");

            var spec = new WishlistItemByUserAndProductSpecification(userId, request.ProductId);
            var existing = await _unitOfWork.GenericRepository<WishlistItem>().GetFirstOrDefaultWithSpecAsync(spec, cancellationToken);
            if (existing is not null)
                throw new BadRequestCustomException("Product is already in wishlist");

            var wishlistItem = new WishlistItem
            {
                UserId = userId,
                ProductId = request.ProductId
            };

            await _unitOfWork.GenericRepository<WishlistItem>().AddAsync(wishlistItem);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Product added to wishlist successfully", StatusCodes.Status200OK);
        }
    }
}

