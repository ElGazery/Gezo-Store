using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Payments.Commands.StripeWebhookCommand
{
    public record StripeWebhookCommand(string Json, string? StripeSignature) : IRequest<ApiResponse<bool>>
    {
    }

    internal class StripeWebhookCommandHandler : IRequestHandler<StripeWebhookCommand, ApiResponse<bool>>
    {
        private readonly IPaymentService _paymentService;

        public StripeWebhookCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ApiResponse<bool>> Handle(StripeWebhookCommand request, CancellationToken cancellationToken)
        {
            await _paymentService.ProcessWebhookAsync(request.Json, request.StripeSignature, cancellationToken);
            return new ApiResponse<bool>(true, "Webhook handled successfully", StatusCodes.Status200OK);
        }
    }
}
