using ChinarAz.Application.DTOs.OrderDtos;
using ChinarAz.Application.DTOs.OrderProductDtos;
using FluentValidation;

namespace ChinarAz.Application.Validations.OrderValidaitons;

public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
{
    public OrderCreateDtoValidator()
    {

        RuleFor(x => x.Products)
            .NotNull().WithMessage("Sifarişdə məhsul olmalıdır.")
            .Must(x => x.Any()).WithMessage("Ən azı bir məhsul sifariş edilməlidir.");
    }
}
