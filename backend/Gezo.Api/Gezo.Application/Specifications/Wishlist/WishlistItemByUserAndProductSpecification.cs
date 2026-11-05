using Gezo.Domain.Entities;
using System;

namespace Gezo.Application.Specifications.Wishlist
{
    public class WishlistItemByUserAndProductSpecification : BaseSpecification<WishlistItem>
    {
        public WishlistItemByUserAndProductSpecification(string userId, int productId)
        {
            Criteria = w => w.UserId == userId && w.ProductId == productId;
        }
    }
}

