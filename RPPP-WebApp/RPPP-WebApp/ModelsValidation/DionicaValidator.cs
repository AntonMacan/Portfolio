using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class DionicaValidator : AbstractValidator<Dionice>
    {
        private const int najduzaDionica = 50; //Nisam siguran da je ovo tocan podatak

        public DionicaValidator()
        {
            RuleFor(d => d.OznakaAutoceste)
                .NotEmpty().WithMessage("Unesite oznaku autoceste.");

            RuleFor(d => d.UlaznaPostaja)
                .NotEmpty().WithMessage("Unesite ulaznu postaju.");

            RuleFor(d => d.IzlaznaPostaja)
                .NotEmpty().WithMessage("Unesite izlaznu postaju.")
                .NotEqual(d => d.UlaznaPostaja).WithMessage("Ulazna i izlazna postaja ne mogu biti iste.");

            //RuleFor(d => d)
            //    .Must(d => d.UlaznaPostajaNavigation.Ime.Contains(d.OznakaAutoceste))
            //    .WithMessage("Ulazna naplatna postaja mora biti na autocesti na kojoj se dionica nalazi.");

            //RuleFor(d => d)
            //    .Must(d => d.IzlaznaPostajaNavigation.Ime.Contains(d.OznakaAutoceste))
            //    .WithMessage("Izlazna naplatna postaja mora biti na autocesti na kojoj se dionica nalazi.");

            RuleFor(d => d.DuljinaKm)
                .NotEmpty().WithMessage("Unesite duljinu dionice.")
                .InclusiveBetween(1, najduzaDionica);
        }
    }
}