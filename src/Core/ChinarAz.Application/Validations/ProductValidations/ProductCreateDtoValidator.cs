using ChinarAz.Application.DTOs.ProductDtos;
using FluentValidation;

namespace ChinarAz.Application.Validations.ProductValidations;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Məhsul adı boş ola bilməz.")
            .MaximumLength(100).WithMessage("Məhsul adı maksimum 100 simvol ola bilər.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId boş ola bilməz.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Qiymət 0-dan böyük olmalıdır.");
    }
}
