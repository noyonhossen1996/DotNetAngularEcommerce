using ECommerceManagement.Application.DTOs.Brands;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface IBrandService
    {
        Task<IReadOnlyList<BrandResponseDto>> GetAllAsync();
        Task<BrandResponseDto?> GetByIdAsync(Guid id);
        Task<BrandResponseDto> CreateAsync(BrandCreateDto dto);
        Task UpdateAsync(Guid id, BrandUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
