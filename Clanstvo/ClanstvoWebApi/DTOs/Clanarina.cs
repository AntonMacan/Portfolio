using System.ComponentModel.DataAnnotations;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{


    public partial class Clanarina
    {
        [Key]
        public int Id { get; set; }
        public bool Placenost { get; set; }
        
        public decimal Iznos { get; set; }
        public int Godina { get; set; }
        public int ClanId { get; set; }
        
        public DateTime? Datum { get; set; }
    }
    public static partial class DtoMapping
    {
        public static Clanarina ToDto(this DomainModels.Clanarina clanarina)
            => new Clanarina()
            {
                Id = clanarina.Id,
                Placenost = clanarina.Placenost,
                Iznos = clanarina.Iznos,
                Godina = clanarina.Godina,
                ClanId = clanarina.ClanId,
                Datum = clanarina.Datum
            };

        public static DomainModels.Clanarina ToDomain(this Clanarina clanarina)
            => new DomainModels.Clanarina(
                clanarina.Id,
                clanarina.Placenost,
                clanarina.Iznos,
                clanarina.Godina,
                clanarina.ClanId,
                clanarina.Datum
            );
    }
}
