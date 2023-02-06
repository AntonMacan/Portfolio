using FluentValidation;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.ModelsValidation;

public class RangStarostValidator : AbstractValidator<RangStarost>
{
    public RangStarostValidator()
    {
        RuleFor(r => r.Naziv)
            .NotEmpty().WithMessage("Potrebno je ime ranga po starosti!");
    }
}