using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Domain.Entities;

namespace ECommerceManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> Users { get; }
        IGenericRepository<RefreshToken> RefreshTokens { get; }
        IProductRepository Products { get; }
        IGenericRepository<ProductImage> ProductImages { get; }
        ICategoryRepository Categories { get; }
        IBrandRepository Brands { get; }

        Task<int> SaveChangesAsync();   
    }
}
