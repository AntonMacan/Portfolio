
using FluentValidation;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.ModelsValidation
{
    public class EncpostajaValidator : AbstractValidator<Encpostaja>
    {
        public EncpostajaValidator()
        {
            RuleFor(enc => enc.VrijemeOtvaranja)

                .NotEqual(enc => enc.VrijemeZatvaranja)
                .WithMessage("Vrijeme otvaranja ne smije biti isto kao vrijeme zatvaranja")

                .NotEmpty().WithMessage("Vrijeme otvaranja je obavezno polje")
                .WithMessage("Polje mora biti u formatu HH:MM");
                    
                //.InclusiveBetween(TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59"))
                //.WithMessage("Vrijeme otvaranja mora biti između 00:00 i 23:59");




            RuleFor(enc => enc.VrijemeZatvaranja)
                .NotEqual(enc => enc.VrijemeOtvaranja)
                .WithMessage("Vrijeme zatvaranja ne smije biti isto kao vrijeme otvaranja")
                .WithMessage("Polje mora biti u formatu HH:MM")

               .NotEmpty().WithMessage("Vrijeme zatvaranja je obavezno polje");
                //.InclusiveBetween(TimeSpan.Parse("00:00"), TimeSpan.Parse("23:59"))
                //.WithMessage("Vrijeme zatvaranja mora biti između 00:00 i 23:59");

          

            RuleFor(enc => enc.KontaktBroj)
                .NotEmpty().WithMessage("Kontankt je obavezno polje");

            RuleFor(enc => enc.Ime)
                .NotEmpty().WithMessage("Kontankt je obavezno polje");

          
               
        }
    }
}
