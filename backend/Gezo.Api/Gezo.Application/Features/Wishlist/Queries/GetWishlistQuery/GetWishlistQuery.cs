using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Wishlist.Dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Application.Specifications.Wishlist;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Wishlist.Queries.GetWishlistQuery
{
    public record GetWishlistQuery : IRequest<ApiResponse<IReadOnlyList<WishlistItemDto>>>
    {
    }

    public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, ApiResponse<IReadOnlyList<WishlistItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetWishlistQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<IReadOnlyList<WishlistItemDto>>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new BadRequestCustomException("User not found");

            var spec = new WishlistWithProductSpecification(userId);
            var items = await _unitOfWork.GenericRepository<WishlistItem>().GetAllWithSpecAsync(spec, cancellationToken);
            var dtos = _mapper.Map<IReadOnlyList<WishlistItemDto>>(items);

            return new ApiResponse<IReadOnlyList<WishlistItemDto>>(dtos, "Wishlist retrieved successfully", StatusCodes.Status200OK);
        }
    }
}

