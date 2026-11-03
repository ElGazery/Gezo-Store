using Gezo.Application.Features.Basket.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface IBasketService
    {
        Task<BasketDto?> GetBasketAsync();

        Task<bool> UpdateBasketAsync(BasketDto basket);

        Task<bool> DeleteBasketAsync();
        Task<bool> DeleteItemAsync(int productId);
    }
}
