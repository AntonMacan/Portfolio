using IzvidaciWebApp.Domain.Models;
using System.Security.Claims;

namespace IzvidaciWebApp.Providers.Http.Dtos;

public class ClanarinaDTO
{
    public int Id { get; set; }
    public bool Placenost { get; set; }

    public decimal Iznos { get; set; }
    public int Godina { get; set; }
    public int ClanId { get; set; }

    public DateTime? Datum { get; set; }

}


public static partial class DtoMapping
{
    public static ClanarinaDTO ToDto(this Domain.Models.Clanarina clanarina)
        => new ClanarinaDTO()
        {
            Id = clanarina.Id,
            Placenost = clanarina.Placenost,
            Iznos = clanarina.Iznos,
            Godina = clanarina.Godina,
            ClanId = clanarina.ClanId,
            Datum = clanarina.Datum
        };

    public static Domain.Models.Clanarina ToDomain(this ClanarinaDTO clanarina)
        => new Domain.Models.Clanarina
        {
            Id = clanarina.Id,
            Placenost = clanarina.Placenost,
            Iznos = clanarina.Iznos,
            Godina = clanarina.Godina,
            ClanId = clanarina.ClanId,
            Datum = clanarina.Datum
        };
}
