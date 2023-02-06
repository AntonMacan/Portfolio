using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class ParkGopsVozValidator : AbstractValidator<ParkGospVoz>
    {

        private const double najsjevernijaTockaHrv = 46.55;
        private const double najjuznijaTockaHrv = 42.383333;
        private const double najistocnijaTockaHrv = 13.5;
        private const double najzapadnijaTockaHrv = 19.45;
        public ParkGopsVozValidator()
        {

            RuleFor(p => p.DionicaId)
                .NotEmpty().WithMessage("Potrebno je odabradi dionicu na kojoj se parkiralište nalazi.");

            RuleFor(p => p.Stacionaža)
                .NotEmpty().WithMessage("Potrebno unijeti stacionažu")
                .InclusiveBetween(0, 999).WithMessage("Metri stacionaže moraju biti manji od 1000"); 

            RuleFor(p => p.Naziv)
                .NotEmpty().WithMessage("Potrebno unijeti naziv parkirališta");

            RuleFor(p => p.BrojMjesta)
                .NotEmpty().WithMessage("Potrebno unijeti broj parkirnih mjesta");


            
                

            RuleFor(p => p.GeoDuzinaUlaz)
                .NotEmpty().WithMessage("Geografska dužina je obavezno polje")
                .InclusiveBetween(najistocnijaTockaHrv, najzapadnijaTockaHrv)
                .WithMessage($"Geografska dužina mora biti između najistočnije({najistocnijaTockaHrv})" +
                $" i najzapadnije({najzapadnijaTockaHrv}) točke Republike Hrvatske");

            RuleFor(p => p.GeoSirinaUlaz)
                .NotEmpty().WithMessage("Geografska širina je obavezno polje")
                .InclusiveBetween(najjuznijaTockaHrv, najsjevernijaTockaHrv)
                .WithMessage($"Geografska širina mora biti između najjužnije({najjuznijaTockaHrv})" +
                $" i najsjevernije({najsjevernijaTockaHrv}) točke Republike Hrvatske"); ;
        }
    }
}
