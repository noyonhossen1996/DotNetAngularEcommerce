using ECommerceManagement.Application.DTOs.Brands;
using FluentValidation;

namespace ECommerceManagement.Application.Validators.Brands
{
    public class BrandUpdateDtoValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
