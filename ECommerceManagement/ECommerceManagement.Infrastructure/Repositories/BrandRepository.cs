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
    public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
    {

        public BrandRepository(ApplicationDbContext context) : base(context)
        { 

        }

        public async Task<bool> IsNameExistsAsync(string name, Guid? excludeBrandId = null)
        {
            return await Context.Brands.AnyAsync(x =>
                x.Name == name &&
                (!excludeBrandId.HasValue || x.Id != excludeBrandId.Value));
        }

    }
}
