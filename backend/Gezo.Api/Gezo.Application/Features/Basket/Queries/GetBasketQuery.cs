using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Basket.Dtos;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Basket.Queries
{
    public record GetBasketQuery:IRequest<ApiResponse<BasketDto>>
    {
    }
    internal class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, ApiResponse<BasketDto>>
    {
        private readonly IBasketService _basketService;

        public GetBasketQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<ApiResponse<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketService.GetBasketAsync();
            if (basket is null)
                throw new NotFoundCustomException("Basket not found ");

            return new ApiResponse<BasketDto>(basket, "Success", StatusCodes.Status200OK);
        }
    }
}
