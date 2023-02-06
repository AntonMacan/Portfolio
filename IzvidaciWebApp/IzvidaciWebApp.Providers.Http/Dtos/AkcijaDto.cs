using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos;
public class AkcijaDto
{
    public int IdAkcije { get; set; }
    public string Naziv { get; set; } = String.Empty;
    public int MjestoPbr { get; set; }
    public int Organizator { get; set; }
    public int KontaktOsoba { get; set; }

    public string? Vrsta { get; set; }

}

    public static partial class DtoMapping
    {
       public static AkcijaDto ToDto(this IzvidaciWebApp.Domain.Models.Akcija akcija)
        {
            return new AkcijaDto()
            {
                IdAkcije = akcija.IdAkcije,
                Naziv = akcija.Naziv,
                MjestoPbr = akcija.MjestoPbr,
                Organizator = akcija.Organizator,
                KontaktOsoba = akcija.KontaktOsoba,
                Vrsta = akcija.Vrsta
            };
        }

        
        public static IzvidaciWebApp.Domain.Models.Akcija ToDomain(this AkcijaDto akcija)
        {
            return new IzvidaciWebApp.Domain.Models.Akcija(akcija.IdAkcije, akcija.Naziv, akcija.MjestoPbr, akcija.Organizator, akcija.KontaktOsoba, akcija.Vrsta);
        }

    }

