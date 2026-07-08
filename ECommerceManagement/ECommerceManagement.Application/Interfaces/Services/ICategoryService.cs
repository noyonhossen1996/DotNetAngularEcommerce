using ECommerceManagement.Application.DTOs.Categories;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto?> GetByIdAsync(Guid id);
        Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto);
        Task UpdateAsync(Guid id, CategoryUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
