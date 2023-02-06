using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class MultimedijaValidator : AbstractValidator<Multimedija>
    {
        public MultimedijaValidator()
        {
            RuleFor(m => m.Naziv).NotEmpty().WithMessage("Unesite naziv!");
            RuleFor(m => m.Url).NotEmpty().WithMessage("Unesite url!");
        }
    }
}
