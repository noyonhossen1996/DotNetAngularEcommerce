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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<bool> IsNameExistsAsync(string name, Guid? excludeCategoryId = null)
        {
            return await Context.Categories.AnyAsync(x =>
                x.Name == name &&
                (!excludeCategoryId.HasValue || x.Id != excludeCategoryId.Value));
        }

    }
}
