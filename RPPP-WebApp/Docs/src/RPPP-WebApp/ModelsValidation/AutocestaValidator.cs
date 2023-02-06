using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class AutocestaValidator : AbstractValidator<Autoceste>
    {
        public AutocestaValidator()
        {
            RuleFor(a => a.Oznaka)
                .NotEmpty().WithMessage("Unesite oznaku autoceste.");

           //Za koncesionara smo dopustili da moze biti nepoznat.

            RuleFor(a => a.Pocetak)
                .NotEmpty().WithMessage("Unesite početak autoceste.");

            RuleFor(a => a.Kraj)
                .NotEmpty().WithMessage("Unesite kraj autoceste.");
        }
    }
}