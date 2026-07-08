using ECommerceManagement.Application.Common;
using ECommerceManagement.Application.DTOs.Products;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductResponseDto>> GetAllAsync();
        Task<PagedResponse<ProductResponseDto>> GetPagedAsync(ProductQueryDto query);
        Task<ProductResponseDto?> GetByIdAsync(Guid id);
        Task<ProductResponseDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
