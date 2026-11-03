using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Basket.Commands.DeleteItemCommand
{
    public record DeleteItemCommand(int productId):IRequest<ApiResponse<bool>>
    {
    }

    internal class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, ApiResponse<bool>>
    {
        private readonly IBasketService _basketService;

        public DeleteItemCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
         var isDeleted=  await _basketService.DeleteItemAsync(request.productId);
            if (!isDeleted)
                throw new BadRequestCustomException("item not deleted");
            return new ApiResponse<bool>(true, "item reomved successfuly", StatusCodes.Status200OK);
        }
    }
}
