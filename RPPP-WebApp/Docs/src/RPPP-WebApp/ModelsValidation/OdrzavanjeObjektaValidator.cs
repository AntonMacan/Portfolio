using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation;

public class OdrzavanjeObjektaValidator : AbstractValidator<OdrzavanjeObjekta>
{
    public OdrzavanjeObjektaValidator()
    {
        RuleFor(o => o.Datum).NotEmpty().WithMessage("Potrebno unijeti datum");
        RuleFor(o => o.Opis).NotEmpty().WithMessage("Potrebno unijeti opis odrzavanja!");
        RuleFor(o => o.Ishod).NotEmpty().WithMessage("Potrebno unijeti ishod!");
        RuleFor(o => o.Odrzavatelj).NotEmpty().WithMessage("Potrebno unijeti odrzavatelja");
        RuleFor(o => o.TipId).NotEmpty().WithMessage("Potrebno odabrati tip odrzavanja!"); 
    }
}
