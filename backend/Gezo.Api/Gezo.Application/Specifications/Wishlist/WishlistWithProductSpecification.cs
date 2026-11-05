using Gezo.Domain.Entities;
using System;

namespace Gezo.Application.Specifications.Wishlist
{
    public class WishlistWithProductSpecification : BaseSpecification<WishlistItem>
    {
        public WishlistWithProductSpecification(string userId)
        {
            Criteria = w => w.UserId == userId;
            AddInclude(w => w.Product);
            AddInclude(w => w.Product.Brand);
            AddInclude(w => w.Product.Images);
        }
    }
}

