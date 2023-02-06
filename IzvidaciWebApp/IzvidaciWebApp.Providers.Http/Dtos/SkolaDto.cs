namespace IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Domain.Models;
public class SkolaDto
{
    public int IdSkole { get; set; }
    public string NazivSkole { get; set; } = String.Empty;
    public int MjestoPbr { get; set; }
    public int Organizator { get; set; }
    public int KontaktOsoba { get; set; }

    }

    public static partial class DtoMapping
    {
        public static SkolaDto ToDto(this Skola skola)
        {
            return new SkolaDto()
            {
                IdSkole = skola.IdSkole,
                NazivSkole = skola.NazivSkole,
                MjestoPbr = skola.MjestoPbr,
                Organizator = skola.Organizator,
                KontaktOsoba = skola.KontaktOsoba,
            };
        }


        public static Skola ToDomain(this SkolaDto skola)
        {
            return new Skola(skola.IdSkole, skola.NazivSkole, skola.MjestoPbr, skola.Organizator, skola.KontaktOsoba);
        }
}
