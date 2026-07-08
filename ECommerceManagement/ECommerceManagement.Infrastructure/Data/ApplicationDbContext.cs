using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Common;
using ECommerceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService? _currentUserService;
            
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService? currentUserService = null) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Brand> Brands => Set<Brand>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService?.UserId?.ToString();
            var entries = ChangeTracker
                .Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = userId;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
