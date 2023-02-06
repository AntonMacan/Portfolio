using Clanstvo.Domain.Models;
using System.ComponentModel.DataAnnotations;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{


    public partial class Clan_NijePlatio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name can't be null")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
        public string Ime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name can't be null")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
        public string Prezime { get; set; } = string.Empty;

        public IEnumerable<Clanarina> Clanarina { get; set; } = Enumerable.Empty<Clanarina>();
    }
    public static partial class DtoMapping
    {
        public static Clan_NijePlatio ToAggregateDto2(this DomainModels.Clan clan)
            => new Clan_NijePlatio()
            {
                Id = clan.Id,
                Ime = clan.Ime,
                Prezime = clan.Prezime,
                Clanarina = clan.Clanarina == null
                                ? new List<Clanarina>()
                                : clan.Clanarina.Where(clanarina => clanarina.Placenost == false)
                                    .Select(pr => pr.ToDto()).ToList()
            };

        /*public static DomainModels.Clan ToDomain(this Clan_NijePlatio clan)
            => new DomainModels.Clan(
                clan.Id,
                clan.Ime,
                clan.Prezime,
                clan.Clanarina.Select(ToDomain)
            );*/
    }
}

