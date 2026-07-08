using AutoMapper;
using ECommerceManagement.Application.DTOs.Brands;
using ECommerceManagement.Application.DTOs.Categories;
using ECommerceManagement.Application.DTOs.ProductImages;
using ECommerceManagement.Application.DTOs.Products;
using ECommerceManagement.Domain.Entities;

namespace ECommerceManagement.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
    .           ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            CreateMap<ProductUpdateDto, Product>();

            CreateMap<ProductImage, ProductImageResponseDto>();

            CreateMap<Category, CategoryResponseDto>();
            CreateMap<CategoryCreateDto, Category>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            CreateMap<CategoryUpdateDto, Category>();

            CreateMap<Brand, BrandResponseDto>();
            CreateMap<BrandCreateDto, Brand>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            CreateMap<BrandUpdateDto, Brand>();

        }
    }
}
