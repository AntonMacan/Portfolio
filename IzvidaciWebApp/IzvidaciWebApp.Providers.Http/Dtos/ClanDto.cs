namespace IzvidaciWebApp.Providers.Http.Dtos;

public class ClanDto
{
    public int Id { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public DateTime DatumRodenja { get; set; }
    public byte[]? Slika { get; set; }
    public string Adresa { get; set; }
    public bool ImaMaramu { get; set; }
    public DateTime? DatumMarama { get; set; }
    public string? MjestoMarama { get; set; }
    public int udrugaId { get; set; }
}


public static partial class DtoMapping
{
    public static ClanDto ToDtoClan(this Domain.Models.Clan r)
        => new ClanDto()
        {
            Id = r.Id,
            Ime = r.Ime,
            Prezime = r.Prezime,
            DatumRodenja = r.DatumRodenja,
            Slika = r.Slika,
            Adresa = r.Adresa,
            ImaMaramu = r.ImaMaramu,
            DatumMarama = r.DatumMarama,
            MjestoMarama = r.MjestoMarama
        };

    public static Domain.Models.Clan ToDomainClan(this ClanDto r)
        => new Domain.Models.Clan(
               r.Id,
               r.Ime,
               r.Prezime,
               r.DatumRodenja,
               r.Slika,
               r.Adresa,
               r.ImaMaramu,
               r.DatumMarama,
               r.MjestoMarama
            );
}