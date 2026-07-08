using AutoMapper;
using ECommerceManagement.Application.DTOs.Categories;
using ECommerceManagement.Application.Exceptions;
using ECommerceManagement.Application.Interfaces;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;

namespace ECommerceManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return _mapper.Map<IReadOnlyList<CategoryResponseDto>>(categories);
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                return null;

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
        {
            var nameExists = await _unitOfWork.Categories.IsNameExistsAsync(dto.Name);

            if (nameExists)
                throw new BadRequestException("Category name already exists.");

            var category = _mapper.Map<Category>(dto);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task UpdateAsync(Guid id, CategoryUpdateDto dto)
        {
            var nameExists = await _unitOfWork.Categories.IsNameExistsAsync(dto.Name, id);

            if (nameExists)
                throw new BadRequestException("Category name already exists.");

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                throw new NotFoundException("Category not found.");

            _mapper.Map(dto, category);

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                throw new NotFoundException("Category not found.");

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
