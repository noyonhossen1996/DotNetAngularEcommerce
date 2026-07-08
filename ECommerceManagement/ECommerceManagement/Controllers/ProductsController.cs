using ECommerceManagement.Application.Common;
using ECommerceManagement.Application.DTOs.ProductImages;
using ECommerceManagement.Application.DTOs.Products;
using ECommerceManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(ApiResponse<IReadOnlyList<ProductResponseDto>>.Ok(products));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] ProductQueryDto query)
        {
            var result = await _productService.GetPagedAsync(query);

            return Ok(ApiResponse<PagedResponse<ProductResponseDto>>.Ok(result));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product is null)
                return NotFound(ApiResponse<ProductResponseDto>.Fail("Product not found."));

            return Ok(ApiResponse<ProductResponseDto>.Ok(product));
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPost("{productId:guid}/images")]
        public async Task<IActionResult> UploadImage(
            Guid productId,
            IFormFile file,
            [FromServices] IProductImageService productImageService)
        {
            var result = await productImageService.UploadAsync(productId, file);

            return Ok(ApiResponse<ProductImageResponseDto>
                .Ok(result, "Image uploaded successfully."));
        }

        [Authorize]
        [HttpGet("{productId:guid}/images")]
        public async Task<IActionResult> GetImages(
            Guid productId,
            [FromServices] IProductImageService productImageService)
        {
            var result = await productImageService.GetByProductIdAsync(productId);

            return Ok(ApiResponse<IReadOnlyList<ProductImageResponseDto>>
                .Ok(result));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("images/{imageId:guid}")]
        public async Task<IActionResult> DeleteImage(
            Guid imageId,
            [FromServices] IProductImageService productImageService)
        {
            await productImageService.DeleteAsync(imageId);

            return Ok(ApiResponse<string>
                .Ok("Image deleted successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productId:guid}/images/{imageId:guid}/set-primary")]
        public async Task<IActionResult> SetPrimaryImage(
            Guid productId,
            Guid imageId,
            [FromServices] IProductImageService productImageService)
        {
            await productImageService.SetPrimaryAsync(productId, imageId);

            return Ok(ApiResponse<string>
                .Ok("Primary image updated successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            var product = await _productService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                ApiResponse<ProductResponseDto>.Ok(product, "Product created successfully."));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ProductUpdateDto dto)
        {
            await _productService.UpdateAsync(id, dto);

            return Ok(ApiResponse<string>.Ok("Product updated successfully."));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);

            return Ok(ApiResponse<string>.Ok("Product deleted successfully."));
        }
    }
}
