using FluentValidation;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.ModelsValidation;

public class ClanarinaValidator : AbstractValidator<Clanarina>
{
    public ClanarinaValidator()
    {
        RuleFor(r => r.Iznos)
            .NotEmpty().WithMessage("Potrebno unijeti iznos članarine!");
        RuleFor(r => r.Placenost)
           .NotEmpty().WithMessage("Potrebno je unijeti plačenost članarine!");
        RuleFor(r => r.Godina)
           .NotEmpty().WithMessage("Potrebno unijeti godinu članarine!");
        RuleFor(r => r.ClanId)
          .NotEmpty().WithMessage("Potrebno unijeti člana!");

    }
}