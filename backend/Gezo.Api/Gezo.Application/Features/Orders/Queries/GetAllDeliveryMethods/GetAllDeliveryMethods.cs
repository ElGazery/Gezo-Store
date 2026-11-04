using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Orders;
using Gezo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Orders.Queries.GetAllDeliveryMethods
{
    public record GetAllDeliveryMethods:IRequest<ApiResponse<ICollection<GetAllDeliveryMethodsDto>>>
    {

    }
    internal class GetAllDeliveryMethodsHandeler : IRequestHandler<GetAllDeliveryMethods, ApiResponse<ICollection<GetAllDeliveryMethodsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDeliveryMethodsHandeler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ICollection<GetAllDeliveryMethodsDto>>> Handle(GetAllDeliveryMethods request, CancellationToken cancellationToken)
        {
            var spec = new GetAllDeliveryMethodsSpecification();
            var data = await _unitOfWork.GenericRepository<DeliveryMethod>().GetAllWithSpecAsync(spec,cancellationToken);
            var dto =  _mapper.Map<ICollection<GetAllDeliveryMethodsDto>>(data);
            return new ApiResponse<ICollection<GetAllDeliveryMethodsDto>>(dto, "Delivery methods retrieved successfully", StatusCodes.Status200OK);
            
        }
    }
}
