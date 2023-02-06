using FluentValidation;
using RPPP_WebApp.Models;
using System.Runtime.ConstrainedExecution;

namespace RPPP_WebApp.ModelsValidation
{
    public class NaplatnaPostajaValidator : AbstractValidator<NaplatnaPostaja>
    {
        private const double najsjevernijaTockaHrv = 46.55;
        private const double najjuznijaTockaHrv = 42.383333;
        private const double najistocnijaTockaHrv = 13.5;
        private const double najzapadnijaTockaHrv = 19.45;

        public NaplatnaPostajaValidator()
        {
            RuleFor(np => np.Ime)
                .NotEmpty().WithMessage("Ime naplatne postaje je obavezno polje")
                .Matches(@"^A[1-9]+[0-9]* [A-Za-z-\p{L}-0-9]+").WithMessage("Ime naplatne postaje mora počinjati oznakom autoceste");


            RuleFor(np => np.GeoDuzina)
                .NotEmpty().WithMessage("Geografska dužina je obavezno polje")
                .InclusiveBetween(najistocnijaTockaHrv, najzapadnijaTockaHrv)
                .WithMessage($"Geografska dužina mora biti između najistočnije({najistocnijaTockaHrv})" +
                $" i najzapadnije({najzapadnijaTockaHrv}) točke Republike Hrvatske");

            RuleFor(np => np.GeoSirina)
                 .NotEmpty().WithMessage("Geografska širina je obavezno polje")
                .InclusiveBetween(najjuznijaTockaHrv, najsjevernijaTockaHrv)
                .WithMessage($"Geografska širina mora biti između najjužnije({najjuznijaTockaHrv})" +
                $" i najsjevernije({najsjevernijaTockaHrv}) točke Republike Hrvatske"); ;
        }
    }
}
