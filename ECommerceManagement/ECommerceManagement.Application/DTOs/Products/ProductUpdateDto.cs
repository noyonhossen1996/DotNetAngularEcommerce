using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Application.DTOs.Products
{
    public class ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }   
    }
}
