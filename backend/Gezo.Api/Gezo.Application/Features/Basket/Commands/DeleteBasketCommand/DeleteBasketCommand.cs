using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Gezo.Application.Features.Basket.Commands.DeleteBasketCommand
{
    public record DeleteBasketCommand: IRequest<ApiResponse<bool>>
    {
    }
    internal class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, ApiResponse<bool>>
    {
        private readonly IBasketService _basketService;

        public DeleteBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            await _basketService.DeleteBasketAsync();
            return new ApiResponse<bool>(true, "Basket deleted successfully", StatusCodes.Status200OK);
        }
    }
}
