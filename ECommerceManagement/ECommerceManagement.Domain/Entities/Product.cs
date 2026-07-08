using ECommerceManagement.Domain.Common;

namespace ECommerceManagement.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string SKU { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid CategoryId { get; set; }

        public Guid BrandId { get; set; }
        public Category Category { get; set; } = null!;
        public Brand Brand { get; set; } = null!;
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
