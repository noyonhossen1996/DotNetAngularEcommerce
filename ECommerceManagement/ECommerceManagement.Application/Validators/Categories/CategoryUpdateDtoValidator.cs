using ECommerceManagement.Application.DTOs.Categories;
using FluentValidation;

namespace ECommerceManagement.Application.Validators.Categories
{
    public class BrandUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
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
