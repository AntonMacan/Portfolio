namespace IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class EdukacijaDto
{
    public int IdEdukacija { get; set; }
    public string NazivEdukacija { get; set; } = string.Empty;
    public int MjestoPbr { get; set; }
    public string OpisEdukacije { get; set; } = string.Empty;
    public int SkolaId { get; set; }
}

public static partial class DtoMapping
{
    public static EdukacijaDto ToDto(this Edukacija edukacija)
    {
        return new EdukacijaDto()
        {
            IdEdukacija = edukacija.Id,
            NazivEdukacija = edukacija.NazivEdukacije,
            MjestoPbr = edukacija.MjestoPbr,
            OpisEdukacije = edukacija.OpisEdukacije,
            SkolaId = edukacija.SkolaId
        };
    }
    public static Edukacija toDomain(this EdukacijaDto edukacija)
    {
        return new Edukacija(edukacija.IdEdukacija, edukacija.NazivEdukacija, edukacija.MjestoPbr, edukacija.OpisEdukacije, edukacija.SkolaId);
    }
}
