using AutoMapper;
using ECommerceManagement.Application.DTOs.ProductImages;
using ECommerceManagement.Application.Exceptions;
using ECommerceManagement.Application.Interfaces;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Application.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public ProductImageService(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ProductImageResponseDto> UploadAsync(Guid productId, IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new BadRequestException("Invalid image file.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new BadRequestException("Only JPG, JPEG, PNG, and WEBP images are allowed.");

            var maxFileSize = 2 * 1024 * 1024; // 2 MB

            if (file.Length > maxFileSize)
                throw new BadRequestException("Image size cannot exceed 2 MB.");

            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            if (product is null)
                throw new NotFoundException("Product not found.");


            var imageUrl = await _fileStorageService.SaveFileAsync(file, "products");

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl,
                IsPrimary = false
            };

            await _unitOfWork.ProductImages.AddAsync(productImage);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductImageResponseDto>(productImage);
        }

        public async Task<IReadOnlyList<ProductImageResponseDto>> GetByProductIdAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            if (product is null)
                throw new NotFoundException("Product not found.");

            var images = await _unitOfWork.ProductImages
                .FindAsync(x => x.ProductId == productId);

            return _mapper.Map<IReadOnlyList<ProductImageResponseDto>>(images);
        }

        public async Task DeleteAsync(Guid imageId)
        {
            var image = await _unitOfWork.ProductImages.GetByIdAsync(imageId);

            if (image is null)
                throw new NotFoundException("Image not found.");

            image.IsDeleted = true;
            image.DeletedAt = DateTime.UtcNow;

            _unitOfWork.ProductImages.Update(image);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SetPrimaryAsync(Guid productId, Guid imageId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            if (product is null)
                throw new NotFoundException("Product not found.");

            var images = await _unitOfWork.ProductImages
                .FindAsync(x => x.ProductId == productId);

            var selectedImage = images.FirstOrDefault(x => x.Id == imageId);

            if (selectedImage is null)
                throw new NotFoundException("Image not found.");

            foreach (var image in images)
            {
                image.IsPrimary = image.Id == imageId;
                _unitOfWork.ProductImages.Update(image);
            }

            await _unitOfWork.SaveChangesAsync();
        }

    }
}
