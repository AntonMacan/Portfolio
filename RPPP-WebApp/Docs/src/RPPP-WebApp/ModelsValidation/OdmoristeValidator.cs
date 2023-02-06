using FluentValidation;
using RPPP_WebApp.Models;
using System;

namespace RPPP_WebApp.ModelsValidation
{
    public class OdmoristeValidator : AbstractValidator<Odmoriste>
    {
        private const double najsjevernijaTockaHrv = 46.55;
        private const double najjuznijaTockaHrv = 42.383333;
        private const double najistocnijaTockaHrv = 13.5;
        private const double najzapadnijaTockaHrv = 19.45;

        public OdmoristeValidator()
        {
            RuleFor(o => o.Naziv)
                .NotEmpty().WithMessage("Unesite naziv odmorišta");

            RuleFor(o => o.DionicaId)
                .NotEmpty().WithMessage("Unesite dionicu na kojoj se nalazi odmorište");

            RuleFor(o => o.StacionazaKm)
                .NotEmpty().WithMessage("Unesite kilometre stacionaže");

            RuleFor(o => o.StacionazaM)
                .NotEmpty().WithMessage("Unesite metre stacionaže")
                .InclusiveBetween(0,999).WithMessage("Metri stacionaže moraju biti manji od 1000");

            RuleFor(o => o.Smjer)
               .NotEmpty().WithMessage("Unesite smjer autoceste s kojeg se pristupa odmorištu");

            RuleFor(o => o.GeografskaDuzina)
                //.InclusiveBetween(najistocnijaTockaHrv, najzapadnijaTockaHrv)
                .Must(duzina =>
                {
                    if (!duzina.HasValue) { return true; }

                    return (duzina >= najistocnijaTockaHrv && duzina <= najzapadnijaTockaHrv);
                })
                .WithMessage($"Geografska dužina mora biti između najistočnije({najistocnijaTockaHrv})" +
                $" i najzapadnije({najzapadnijaTockaHrv}) točke Republike Hrvatske");

            RuleFor(o => o.GeografskaSirina)
                //.InclusiveBetween(najjuznijaTockaHrv, najsjevernijaTockaHrv)
                .Must(sirina =>
                {
                    if (!sirina.HasValue) { return true; }

                    return (sirina >= najjuznijaTockaHrv && sirina <= najsjevernijaTockaHrv);
                })
                .WithMessage($"Geografska širina mora biti između najjužnije({najjuznijaTockaHrv})" +
                $" i najsjevernije({najsjevernijaTockaHrv}) točke Republike Hrvatske");

        }
    }
}
