using ECommerceManagement.Application.Common;
using ECommerceManagement.Application.DTOs.Categories;
using ECommerceManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(ApiResponse<IReadOnlyList<CategoryResponseDto>>.Ok(categories));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category is null)
                return NotFound(ApiResponse<CategoryResponseDto>.Fail("Category not found."));

            return Ok(ApiResponse<CategoryResponseDto>.Ok(category));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            var category = await _categoryService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                ApiResponse<CategoryResponseDto>.Ok(category, "Category created successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CategoryUpdateDto dto)
        {
            await _categoryService.UpdateAsync(id, dto);

            return Ok(ApiResponse<string>.Ok("Category updated successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok(ApiResponse<string>.Ok("Category deleted successfully."));
        }
    }
}
