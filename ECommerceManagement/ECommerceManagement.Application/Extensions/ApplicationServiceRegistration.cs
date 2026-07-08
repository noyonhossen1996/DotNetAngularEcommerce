using ECommerceManagement.Application.Interfaces.Auth;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Application.Services;
using ECommerceManagement.Application.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceManagement.Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(ApplicationServiceRegistration).Assembly);
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();

            return services;
        }
    }
}
