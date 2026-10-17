using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Brands.Queries.GetAllBrandsQuery
{
    public class GetAllBrandsQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string? LogoUrl { get; set; }
    }
}
