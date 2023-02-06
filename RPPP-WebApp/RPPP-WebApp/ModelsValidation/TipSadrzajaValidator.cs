using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class TipSadrzajaValidator : AbstractValidator<TipSadrzaja>
    {
        public TipSadrzajaValidator()
        {
            RuleFor(t => t.Naziv).NotEmpty().WithMessage("Unesite naziv!");
        }
    }
}
