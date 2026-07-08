using ECommerceManagement.Application.DTOs.Categories;
using FluentValidation;

namespace ECommerceManagement.Application.Validators.Categories
{
    public class BrandCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public BrandCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
