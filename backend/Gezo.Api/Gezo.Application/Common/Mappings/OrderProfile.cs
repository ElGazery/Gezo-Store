using AutoMapper;
using Gezo.Application.Features.Orders.Commands.AddOrderCommand;
using Gezo.Application.Features.Orders.Dtos;
using Gezo.Application.Features.Orders.Queries.GetAllDeliveryMethods;
using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Common.Mappings
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<DeliveryMethod, GetAllDeliveryMethodsDto>();
            CreateMap<AddOrderCommand, Order>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));

            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
