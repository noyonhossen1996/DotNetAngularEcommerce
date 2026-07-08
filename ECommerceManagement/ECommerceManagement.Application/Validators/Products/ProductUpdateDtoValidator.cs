using ECommerceManagement.Application.DTOs.Products;
using FluentValidation;

namespace ECommerceManagement.Application.Validators.Products
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.SKU)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.BrandId)
                .NotEmpty();
        }
    }
}
