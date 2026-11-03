using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Basket.Dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Application.Specifications.Products;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Basket.Commands.UpdateBasketCommand
{
    public record UpdateBasketCommand(int productId, int quantity) : IRequest<ApiResponse<bool>>
    {
    }
    internal class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, ApiResponse<bool>>
    {
        private const int MaxQuantity = 99;

        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBasketCommandHandler(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper )
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
        {
            var spec = new ProductByIdSpecification(request.productId);
            var product = await _unitOfWork.GenericRepository<Product>().GetFirstOrDefaultWithSpecAsync(spec,cancellationToken);

            if (product is null )
                throw new NotFoundCustomException("Product Not found");

            
            var basket = await _basketService.GetBasketAsync() ?? new BasketDto();
             
            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == request.productId);
            if (existingItem is not null)
            {
                var newQuantity =request.quantity;

                if (newQuantity > MaxQuantity)
                    throw new BadRequestCustomException($"Maximum quantity is {MaxQuantity}");

                existingItem.Quantity = newQuantity;
            }
            else
            {
                if (request.quantity > MaxQuantity)
                    throw new BadRequestCustomException($"Maximum quantity is {MaxQuantity}");

                var basketItem = _mapper.Map<BasketItemDto>(product);
                basketItem.Quantity = request.quantity;
                basket.Items.Add(basketItem);
            }


            var isUpdated = await _basketService.UpdateBasketAsync(basket);

            if (!isUpdated)
                throw new BadRequestCustomException("Failed to update basket");


            return new ApiResponse<bool>(true, "Item Add to basket successfuly", StatusCodes.Status200OK);

        }
    }
}
