using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class ObavijestValidator : AbstractValidator<Obavijesti>
    {
        public ObavijestValidator()
        {
            RuleFor(o => o.Naslov)
                .NotEmpty().WithMessage("Unesite naslov obavijesti.");

            RuleFor(o => o.Opis)
                .NotEmpty().WithMessage("Obavijest mora imati opis.");
        }
    }
}