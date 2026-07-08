using AutoMapper;
using ECommerceManagement.Application.Common;
using ECommerceManagement.Application.DTOs.Products;
using ECommerceManagement.Application.Exceptions;
using ECommerceManagement.Application.Interfaces;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProductResponseDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResponseDto>>(products);
            return mappedProducts;  
        }

        public async Task<PagedResponse<ProductResponseDto>> GetPagedAsync(ProductQueryDto query)
        {
            //var productsQuery = _unitOfWork.Products
            //    .GetQueryable()
            //    .AsNoTracking();

            //if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            //{
            //    productsQuery = productsQuery.Where(x =>
            //        x.Name.Contains(query.SearchTerm) ||
            //        x.SKU.Contains(query.SearchTerm));
            //}

            //if (query.MinPrice.HasValue)
            //    productsQuery = productsQuery.Where(x => x.Price >= query.MinPrice.Value);

            //if (query.MaxPrice.HasValue)
            //    productsQuery = productsQuery.Where(x => x.Price <= query.MaxPrice.Value);

            //if (query.IsActive.HasValue)
            //    productsQuery = productsQuery.Where(x => x.IsActive == query.IsActive.Value);

            //productsQuery = query.SortBy?.ToLower() switch
            //{
            //    "price" => query.SortDescending
            //        ? productsQuery.OrderByDescending(x => x.Price)
            //        : productsQuery.OrderBy(x => x.Price),

            //    "name" => query.SortDescending
            //        ? productsQuery.OrderByDescending(x => x.Name)
            //        : productsQuery.OrderBy(x => x.Name),

            //    _ => productsQuery.OrderByDescending(x => x.CreatedAt)
            //};

            //var totalCount = await productsQuery.CountAsync();

            //var items = await productsQuery
            //    .Skip((query.PageNumber - 1) * query.PageSize)
            //    .Take(query.PageSize)
            //    .Select(x => new ProductResponseDto
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        Description = x.Description,
            //        SKU = x.SKU,
            //        Price = x.Price,
            //        IsActive = x.IsActive
            //    })
            //    .ToListAsync();

            //return new PagedResponse<ProductResponseDto>
            //{
            //    Items = items,
            //    PageNumber = query.PageNumber,
            //    PageSize = query.PageSize,
            //    TotalCount = totalCount
            //};
            return new();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product is null)
                return null;

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
        {
            var skuExists = await _unitOfWork.Products.IsSkuExistsAsync(dto.SKU);

            if (skuExists)
                throw new BadRequestException("Product SKU already exists.");

            var product = _mapper.Map<Product>(dto);

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product is null)
                throw new NotFoundException("Product not found.");

            var skuExists = await _unitOfWork.Products.IsSkuExistsAsync(dto.SKU, id);

            if (skuExists)
                throw new BadRequestException("Product SKU already exists.");

            _mapper.Map(dto, product);

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product is null)
                throw new NotFoundException("Product not found.");

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
