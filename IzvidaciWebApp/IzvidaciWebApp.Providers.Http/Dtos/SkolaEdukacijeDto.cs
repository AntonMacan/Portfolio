using IzvidaciWebApp.Domain.Models;
namespace IzvidaciWebApp.Providers.Http.Dtos;
public class SkolaEdukacijeDto
{
    public int IdSkole { get; set; }
    public string NazivSkole { get; set; } = String.Empty;
    public int MjestoPbr { get; set; }
    public int Organizator { get; set; }
    public int KontaktOsoba { get; set; }
    public IEnumerable<EdukacijaDto> EdukacijeUSkoli { get; set; } = Enumerable.Empty<EdukacijaDto>();

}

    public static partial class DtoMapping
    {
    public static SkolaEdukacijeDto ToDtoAggregate(this Skola skola)
    {
        return new SkolaEdukacijeDto()
        {
            IdSkole = skola.IdSkole,
            NazivSkole = skola.NazivSkole,
            MjestoPbr = skola.MjestoPbr,
            Organizator = skola.Organizator,
            KontaktOsoba = skola.KontaktOsoba,
            EdukacijeUSkoli = skola.EdukacijeUSkoli == null ? new List<EdukacijaDto>() : skola.EdukacijeUSkoli.Select(edukacija => edukacija.ToDto()).ToList()
        };
    }
    public static Skola ToDomainAggregate(this SkolaEdukacijeDto skola)
    {
        return new Skola(skola.IdSkole, skola.NazivSkole, skola.MjestoPbr, skola.Organizator, skola.KontaktOsoba, skola.EdukacijeUSkoli.Select(toDomain));
    }
}
