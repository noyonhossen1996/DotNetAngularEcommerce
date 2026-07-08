using ECommerceManagement.Application.DTOs.Products;
using FluentValidation;

namespace ECommerceManagement.Application.Validators.Products
{
    public class ProductQueryDtoValidator : AbstractValidator<ProductQueryDto>
    {
        public ProductQueryDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
                .WithMessage("MinPrice cannot be greater than MaxPrice.");

            RuleFor(x => x.SortBy)
                .Must(x => string.IsNullOrWhiteSpace(x) ||
                           x.ToLower() == "name" ||
                           x.ToLower() == "price")
                .WithMessage("SortBy must be either 'name' or 'price'.");
        }
    }
}
