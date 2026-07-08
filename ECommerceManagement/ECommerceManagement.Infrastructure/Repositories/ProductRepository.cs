using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Domain.Entities;
using ECommerceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {

        public ProductRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Product?> GetByIdAsync(Guid id)
        {
            return await Context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await Context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Images)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsSkuExistsAsync(string sku, Guid? excludeProductId = null)
        {
            return await Context.Products.AnyAsync(x =>
                x.SKU == sku &&
                (!excludeProductId.HasValue || x.Id != excludeProductId.Value));
        }

    }
}
