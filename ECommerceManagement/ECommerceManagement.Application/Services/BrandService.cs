using AutoMapper;
using ECommerceManagement.Application.DTOs.Brands;
using ECommerceManagement.Application.Exceptions;
using ECommerceManagement.Application.Interfaces;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;

namespace ECommerceManagement.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BrandResponseDto>> GetAllAsync()
        {
            var brands = await _unitOfWork.Brands.GetAllAsync();

            return _mapper.Map<IReadOnlyList<BrandResponseDto>>(brands);
        }

        public async Task<BrandResponseDto?> GetByIdAsync(Guid id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);

            if (brand is null)
                return null;

            return _mapper.Map<BrandResponseDto>(brand);
        }

        public async Task<BrandResponseDto> CreateAsync(BrandCreateDto dto)
        {
            var nameExists = await _unitOfWork.Brands.IsNameExistsAsync(dto.Name);

            if (nameExists)
                throw new BadRequestException("Brand name already exists.");

            var brand = _mapper.Map<Brand>(dto);

            await _unitOfWork.Brands.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BrandResponseDto>(brand);
        }

        public async Task UpdateAsync(Guid id, BrandUpdateDto dto)
        {
            var nameExists = await _unitOfWork.Brands.IsNameExistsAsync(dto.Name, id);

            if (nameExists)
                throw new BadRequestException("Brand name already exists.");

            var brand = await _unitOfWork.Brands.GetByIdAsync(id);

            if (brand is null)
                throw new NotFoundException("Brand not found.");

            _mapper.Map(dto, brand);

            _unitOfWork.Brands.Update(brand);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);

            if (brand is null)
                throw new NotFoundException("Brand not found.");

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Brands.Update(brand);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
