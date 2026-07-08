using ECommerceManagement.Application.DTOs.ProductImages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface IProductImageService
    {
        Task<ProductImageResponseDto> UploadAsync(Guid productId, IFormFile file);
        Task<IReadOnlyList<ProductImageResponseDto>> GetByProductIdAsync(Guid productId);
        Task DeleteAsync(Guid imageId);
        Task SetPrimaryAsync(Guid productId, Guid imageId);
    }
}
