using ChinarAz.Application.DTOs.OrderProductDtos;
using FluentValidation;

namespace ChinarAz.Application.Validations.OrderProductValidations;

public class OrderProductCreateDtoValidator : AbstractValidator<OrderProductCreateDto>
{
    public OrderProductCreateDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Məhsul seçilməlidir.");

        // Ədəd ilə alınan məhsul üçün (IsWeighted=false)
        When(x => x.WeightGram == 0, () =>
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Ədəd 0-dan böyük olmalıdır.")
                .LessThanOrEqualTo(10).WithMessage("Maksimum 10 ədəd ala bilərsiniz.");
        });

        // Çəki ilə alınan məhsul üçün (IsWeighted=true)
        When(x => x.Quantity == 0, () =>
        {
            RuleFor(x => x.WeightGram)
                .GreaterThan(0).WithMessage("Çəki 0-dan böyük olmalıdır.")
                .LessThanOrEqualTo(2000).WithMessage("Maksimum 1000 qram ala bilərsiniz.");
        });

        // Hər ikisi sıfır ola bilməz
        RuleFor(x => x)
            .Must(x => x.Quantity > 0 || x.WeightGram > 0)
            .WithMessage("Ən azı miqdar və ya qram göstərilməlidir.");
    }
}
