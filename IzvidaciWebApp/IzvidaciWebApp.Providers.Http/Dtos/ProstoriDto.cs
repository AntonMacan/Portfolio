using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos;

public class ProstoriDto
{
    public int id { get; set; }
    public int idUdruge { get; set; }
    public string adresa { get; set; }
    public string namjena { get; set; }
    public string dodijelio { get; set; }
    public DateTime? dodjeljenoDo { get; set; }
    public decimal? geoDuzina { get; set; }
    public decimal? geoSirina { get; set; }


    
}

public static partial class DtoMapping
{
    public static ProstoriDto ToDto(this Domain.Models.Prostori prostor)
        => new ProstoriDto()
        {
            id = prostor.Id,
            idUdruge = prostor.IdUdruge,
            adresa = prostor.Adresa,
            namjena = prostor.Namjena,
            dodijelio = prostor.Dodijelio,
            dodjeljenoDo = prostor.DodjeljenoDo,
            geoDuzina = prostor.GeoDuzina,
            geoSirina = prostor.GeoSirina
        };

    public static Domain.Models.Prostori ToDomain(this ProstoriDto prostori
    )
        => new Domain.Models.Prostori(
            prostori.id,
            prostori.idUdruge,
            prostori.adresa,
            prostori.namjena,
            prostori.dodijelio,
            prostori.dodjeljenoDo,
            prostori.geoDuzina,
            prostori.geoSirina);
}