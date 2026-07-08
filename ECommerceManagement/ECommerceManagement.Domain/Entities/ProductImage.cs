using ECommerceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Domain.Entities
{
    public class ProductImage : AuditableEntity
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }
    }
}
