using FluentValidation;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.ViewModels;

namespace IzvidaciWebApp.ModelsValidation;

public class RangZaslugaValidator : AbstractValidator<RangZasluga>
{
    public RangZaslugaValidator()
    {
        RuleFor(r => r.Naziv)
            .NotEmpty().WithMessage("Potrebno je ime ranga po zasluzi!");
    }
}