using Clanstvo.Domain.Models;
using Clanstvo.Providers.Http.DTOs;

namespace ClanstvoWebApi.DTOs;

public class Skola
{
    public int IdSkole { get; set; }

    public string Naziv { get; set; } = string.Empty;


}

public static partial class DtoMapping
{
    public static SkolaSudionik ToDto(this Clanstvo.Domain.Models.Skola skola)
    {
        return new SkolaSudionik()
        {
            IdSkole = skola.Id,
            NazivSkole = skola.NazivSkole
        };
    }


    /*
    public static Domain.Models.Akcija ToDomain(AkcijaSudionik akcija)
    {
            return new Domain.Models.Akcija(akcija.IdAkcije, akcija.Naziv);
    }
    */
}


