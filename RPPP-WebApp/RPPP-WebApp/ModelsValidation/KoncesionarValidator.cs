using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class KoncesionarValidator : AbstractValidator<Koncesionari>
    {
        public KoncesionarValidator()
        {
            RuleFor(k => k.NazivKoncesionara)
                .NotEmpty().WithMessage("Unesite naziv koncesionara.");

            RuleFor(k => k.Url)
                .Must(url => url.Length == 0 || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Url konceionara nije dobro formatiran.");
        }
    }
}