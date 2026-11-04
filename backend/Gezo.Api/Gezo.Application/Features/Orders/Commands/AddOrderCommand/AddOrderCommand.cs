using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Basket.Dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Orders.Commands.AddOrderCommand
{
    public record AddOrderCommand :IRequest<ApiResponse<bool>>
    {
         public int DeliveryMethodId { get; set; }
        public string ShipToName { get; set; } 

        public string ShipToPhone { get; set; } 

        public string ShipToAddress { get; set; } 

        public string ShipToCity { get; set; } 

        public string? ShipToPostalCode { get; set; }
    }


    internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBasketService _basketService;
        public AddOrderCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IBasketService basketService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _basketService = basketService;
        }

        public async Task<ApiResponse<bool>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var deliveryMethod = await _unitOfWork.GenericRepository<DeliveryMethod>().GetbyIdAsync(request.DeliveryMethodId);
            if (deliveryMethod is null)
                throw new BadRequestCustomException("Delivery Method not found ");
            var order = _mapper.Map<Order>(request);
            //1 generat order number
            order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(100000, 999999)}";
            //2 get user and asign to the order
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new BadRequestCustomException("User not found");
            order.UserId = userId;
            order.DeliveryMethodName = deliveryMethod.Name;
            order.DeliveryMethodCost = deliveryMethod.Cost;
            var basket = await _basketService.GetBasketAsync();
            if (basket is null)
                throw new BadRequestCustomException("The basket not found!");
            var items = basket.Items;
            if (items is null || items.Count == 0)
                throw new BadRequestCustomException("Basket is empty!");
            var orderItems = _mapper.Map<ICollection<OrderItem>>(items);

            //3 check product and stock and calculate subtotal
            foreach (var i in orderItems)
            {
                var product = await _unitOfWork.GenericRepository<Product>().GetbyIdAsync(i.ProductId);
                if (product is null)
                    throw new BadRequestCustomException("Product not found");

                if (product.StockQuantity < i.Quantity)
                    throw new BadRequestCustomException($"Product {product.Name} is out of stock");

                product.StockQuantity -= i.Quantity;

                order.Subtotal += i.TotalPrice;
            }

            order.Items = orderItems;
            order.Total = order.Subtotal + deliveryMethod.Cost;

            await _unitOfWork.GenericRepository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _basketService.DeleteBasketAsync();

            return new ApiResponse<bool>(true, "Order Created Successfuly", StatusCodes.Status201Created);
        }
    }
}
