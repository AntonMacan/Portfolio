using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation;

public class KameraValidator : AbstractValidator<Kamere>
{
    private const double najsjevernijaTockaHrv = 46.55;
    private const double najjuznijaTockaHrv = 42.383333;
    private const double najistocnijaTockaHrv = 13.5;
    private const double najzapadnijaTockaHrv = 19.45;

    public KameraValidator()
    {
        RuleFor(k => k.DionicaId)
            .NotEmpty().WithMessage("Potrebno je odabrati dionicu na kojoj se kamera nalazi.");
        RuleFor(k => k.Naziv)
            .NotEmpty().WithMessage("Potrebno unijeti naziv kamere.");
        RuleFor(k => k.GeografskaDuzina)
            .InclusiveBetween(najistocnijaTockaHrv,najzapadnijaTockaHrv)
            .WithMessage($"Geografska dužina mora biti između najistočnije({najistocnijaTockaHrv})" +
            $" i najzapadnije({najzapadnijaTockaHrv}) točke Republike Hrvatske");
        RuleFor(k => k.GeografskaSirina)
            .InclusiveBetween(najjuznijaTockaHrv, najsjevernijaTockaHrv)
            .WithMessage($"Geografska širina mora biti između najjužnije({najjuznijaTockaHrv})" +
                         $" i najsjevernije({najsjevernijaTockaHrv}) točke Republike Hrvatske"); ;
    }
}
