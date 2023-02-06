using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using System.ComponentModel.DataAnnotations;

namespace IzvidaciWebApp.Providers.Http.Dtos;
public class EdukacijaAggregateDto
{
    public int IdEdukacija { get; set; }
    public string NazivEdukacija { get; set; } = string.Empty;
    public int MjestoPbr { get; set; }
    public string OpisEdukacije { get; set; } = String.Empty;
    public int SkolaId { get; set; }
    public IEnumerable<PredavacNaEdukacijiDto> PredavaciNaEdukaciji { get; set; } = Enumerable.Empty<PredavacNaEdukacijiDto>();
    public IEnumerable<PolaznikNaEdukacijiDto> PolazniciEdukacije { get; set; } = Enumerable.Empty<PolaznikNaEdukacijiDto>();
    public IEnumerable<PrijavljenClanNaEdukacijuDto> PrijavljeniNaEdukaciju { get; set; } = Enumerable.Empty<PrijavljenClanNaEdukacijuDto>();
}

public static partial class DtoMapping
{
    public static EdukacijaAggregateDto ToAggregateDto(this Edukacija edukacija)
    {
        return new EdukacijaAggregateDto()
        {
            IdEdukacija = edukacija.Id,
            NazivEdukacija = edukacija.NazivEdukacije,
            MjestoPbr = edukacija.MjestoPbr,
            OpisEdukacije = edukacija.OpisEdukacije,
            SkolaId = edukacija.SkolaId,
            PredavaciNaEdukaciji = edukacija.PredavaciNaEdukaciji == null ? new List<PredavacNaEdukacijiDto>() : edukacija.PredavaciNaEdukaciji.Select(predavac => predavac.ToDto()).ToList(),
            PolazniciEdukacije = edukacija.PolazniciEdukacije == null ? new List<PolaznikNaEdukacijiDto>() : edukacija.PolazniciEdukacije.Select(polaznik => polaznik.ToDto()).ToList(),
            PrijavljeniNaEdukaciju = edukacija.PrijavljeniNaEdukaciji == null ? new List<PrijavljenClanNaEdukacijuDto>() : edukacija.PrijavljeniNaEdukaciji.Select(prijavljeni => prijavljeni.ToDto()).ToList()
        };
    }
    public static Edukacija toDomainAggregate(this EdukacijaAggregateDto edukacija)
    {
        return new Edukacija(edukacija.IdEdukacija, edukacija.NazivEdukacija, edukacija.MjestoPbr, edukacija.OpisEdukacije, edukacija.SkolaId, edukacija.PredavaciNaEdukaciji.Select(ToDomain), edukacija.PolazniciEdukacije.Select(ToDomain), edukacija.PrijavljeniNaEdukaciju.Select(ToDomain));
    }


}