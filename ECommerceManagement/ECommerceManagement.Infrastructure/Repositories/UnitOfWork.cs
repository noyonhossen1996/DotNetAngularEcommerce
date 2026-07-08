using ECommerceManagement.Application.Interfaces;
using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Domain.Entities;
using ECommerceManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new GenericRepository<ApplicationUser>(_context);
            RefreshTokens = new GenericRepository<RefreshToken>(_context);
            Products = new ProductRepository(_context);
            ProductImages = new GenericRepository<ProductImage>(_context);
            Categories = new CategoryRepository(_context);
            Brands = new BrandRepository(_context);
        }

        public IGenericRepository<ApplicationUser> Users { get; }
        public IGenericRepository<RefreshToken> RefreshTokens { get; }
        public IProductRepository Products { get; }
        public IGenericRepository<ProductImage> ProductImages { get; }
        public ICategoryRepository Categories { get; }
        public IBrandRepository Brands { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
