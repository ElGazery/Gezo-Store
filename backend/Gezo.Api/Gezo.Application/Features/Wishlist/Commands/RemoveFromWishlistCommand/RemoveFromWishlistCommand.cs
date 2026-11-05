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

namespace Gezo.Application.Features.Wishlist.Commands.RemoveFromWishlistCommand
{
    public record RemoveFromWishlistCommand(int ProductId) : IRequest<ApiResponse<bool>>
    {
    }

    public class RemoveFromWishlistCommandHandler : IRequestHandler<RemoveFromWishlistCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RemoveFromWishlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<bool>> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new BadRequestCustomException("User not found");

            var spec = new WishlistItemByUserAndProductSpecification(userId, request.ProductId);
            var item = await _unitOfWork.GenericRepository<WishlistItem>().GetFirstOrDefaultWithSpecAsync(spec, cancellationToken);
            if (item is null)
                throw new NotFoundCustomException("Product not found in wishlist");

            await _unitOfWork.GenericRepository<WishlistItem>().DeleteAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Product removed from wishlist successfully", StatusCodes.Status200OK);
        }
    }
}

