
using Gezo.Application.Features.Basket.Dtos;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text.Json;

namespace Gezo.Infrastructure.Services;

public class BasketService : IBasketService
{
    private readonly IDatabase _database;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BasketService(
        IConnectionMultiplexer redis,
        IHttpContextAccessor httpContextAccessor)
    {
        _database = redis.GetDatabase();
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetBasketId()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null)
            throw new InvalidOperationException(
                "HttpContext is unavailable");

        var user = context.User;

        // Logged-in User
        if (user.Identity?.IsAuthenticated == true)
        {
            var userId = user.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException();

            return userId;
        }

        // Guest with existing Cookie
        if (context.Request.Cookies.TryGetValue(
            "basketId", out var basketId)
            && !string.IsNullOrWhiteSpace(basketId))
        {
            return basketId;
        }

        // New Guest
        basketId = Guid.NewGuid().ToString();

        context.Response.Cookies.Append(
            "basketId",
            basketId,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                MaxAge = TimeSpan.FromDays(14),
                IsEssential = true
            });

        return basketId;
    }

    public async Task<bool> UpdateBasketAsync(BasketDto basket)
    {
        var basketId = GetBasketId();

        basket.Id = basketId;

        var data = JsonSerializer.Serialize(basket);

        return await _database.StringSetAsync(
            $"basket:{basketId}",
            data,
            TimeSpan.FromDays(14));
    }

    public async Task<BasketDto?> GetBasketAsync()
    {
        var basketId = GetBasketId();

        var data = await _database.StringGetAsync(
            $"basket:{basketId}");

        if (data.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<BasketDto>(
            data.ToString());
    }

    public async Task<bool> DeleteBasketAsync()
    {
        var basketId = GetBasketId();

        return await _database.KeyDeleteAsync(
            $"basket:{basketId}");
    }
    public async Task<bool> DeleteItemAsync(int productId)
    {
        var basketId = GetBasketId();
        var data = await _database.StringGetAsync($"basket:{basketId}");
        if (data.IsNullOrEmpty)
            throw new NotFoundCustomException("basket not found");

        var basket = JsonSerializer.Deserialize<BasketDto>(data.ToString());
        var items = basket.Items.ToList();
        var product = items.FirstOrDefault(p => p.ProductId ==productId);
        if (product is null)
            throw new NotFoundCustomException("Product not found in basket");

        basket.Items.Remove(product);
        var newBasket = JsonSerializer.Serialize(basket);              
      return  await _database.StringSetAsync(
            $"basket:{basketId}",
            newBasket,
            TimeSpan.FromDays(14));
    }
}
