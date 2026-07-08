using ECommerceManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Application.Interfaces.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<bool> IsSkuExistsAsync(string sku, Guid? excludeProductId = null);
    }
}
