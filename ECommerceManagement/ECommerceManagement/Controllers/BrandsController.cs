using ECommerceManagement.Application.Common;
using ECommerceManagement.Application.DTOs.Brands;
using ECommerceManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService BrandService)
        {
            _brandService = BrandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _brandService.GetAllAsync();
            return Ok(ApiResponse<IReadOnlyList<BrandResponseDto>>.Ok(categories));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var brand = await _brandService.GetByIdAsync(id);

            if (brand is null)
                return NotFound(ApiResponse<BrandResponseDto>.Fail("Brand not found."));

            return Ok(ApiResponse<BrandResponseDto>.Ok(brand));
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateDto dto)
        {
            var brand = await _brandService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = brand.Id },
                ApiResponse<BrandResponseDto>.Ok(brand, "Brand created successfully."));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, BrandUpdateDto dto)
        {
            await _brandService.UpdateAsync(id, dto);

            return Ok(ApiResponse<string>.Ok("Brand updated successfully."));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _brandService.DeleteAsync(id);

            return Ok(ApiResponse<string>.Ok("Brand deleted successfully."));
        }
    }
}
