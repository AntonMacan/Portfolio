using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class SadrzajValidator : AbstractValidator<Sadrzaj>
    {
        private const double najsjevernijaTockaHrv = 46.55;
        private const double najjuznijaTockaHrv = 42.383333;
        private const double najistocnijaTockaHrv = 13.5;
        private const double najzapadnijaTockaHrv = 19.45;

        private readonly RPPP05Context context;
        public SadrzajValidator(RPPP05Context context)
        {
            this.context = context;

            RuleFor(s => s.OdmoristeId)
                .NotEmpty()
                .WithMessage("Unesite odmorište");

            RuleFor(s => s.Naziv)
                .NotEmpty()
                .WithMessage("Unesite naziv sadržaja");

            RuleFor(s => s.TipSadrzajaId)
                .NotEmpty()
                .WithMessage("Odaberite tip sadržaja");

            RuleFor(s => s.GeografskaDuzina)
                .Must(duzina =>
                {
                    if (!duzina.HasValue) { return true; }

                    return (duzina >= najistocnijaTockaHrv && duzina <= najzapadnijaTockaHrv);
                })
                .WithMessage($"Geografska dužina mora biti između najistočnije({najistocnijaTockaHrv})" +
                $" i najzapadnije({najzapadnijaTockaHrv}) točke Republike Hrvatske");

            RuleFor(s => s.GeografskaSirina)
                .Must(sirina =>
                {
                    if (!sirina.HasValue) { return true; }

                    return (sirina >= najjuznijaTockaHrv && sirina <= najsjevernijaTockaHrv);
                })
                .WithMessage($"Geografska širina mora biti između najjužnije({najjuznijaTockaHrv})" +
                $" i najsjevernije({najsjevernijaTockaHrv}) točke Republike Hrvatske");

            RuleFor(s => s)
                .Must(sadrzaj =>
                {
                    if (!sadrzaj.GeografskaDuzina.HasValue && !sadrzaj.GeografskaSirina.HasValue)
                    {
                        return true;
                    }

                    if((sadrzaj.GeografskaDuzina.HasValue && !sadrzaj.GeografskaSirina.HasValue)
                    || (!sadrzaj.GeografskaDuzina.HasValue && sadrzaj.GeografskaSirina.HasValue))
                    {
                        return false;
                    }

                    var odmoriste = context.Odmoriste.Find(sadrzaj.OdmoristeId);

                    if(!odmoriste.GeografskaDuzina.HasValue || !odmoriste.GeografskaSirina.HasValue)
                    {
                        return true;
                    }

                    double udaljenost = Math.Sqrt(Math.Pow(sadrzaj.GeografskaDuzina.Value - odmoriste.GeografskaDuzina.Value, 2)
                        + Math.Pow(sadrzaj.GeografskaSirina.Value - odmoriste.GeografskaSirina.Value, 2));

                    if (udaljenost < (2.0/111))
                    {
                        return true;
                    }

                    return false;
                })
                .WithMessage("Koordinate sadržaja moraju biti udaljene manje od 2 kilometra od koordinata odmorišta.");
        }
    }
}
