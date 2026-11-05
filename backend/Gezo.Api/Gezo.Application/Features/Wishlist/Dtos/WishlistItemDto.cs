namespace Gezo.Application.Features.Wishlist.Dtos
{
    public class WishlistItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string? PictureUrl { get; set; }

        public string BrandName { get; set; } = string.Empty;

        public bool InStock { get; set; }
    }
}

