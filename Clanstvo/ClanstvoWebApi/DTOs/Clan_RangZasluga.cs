using Clanstvo.Domain.Models;
using System.ComponentModel.DataAnnotations;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{
    public partial class Clan_RangZasluga
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name can't be null")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
        public string Ime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name can't be null")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
        public string Prezime { get; set; } = string.Empty;

        //public IEnumerable<DodjelaZasluga> DodjeleZasluga { get; set; } = Enumerable.Empty<DodjelaZasluga>();


        public int? RangId { get; set; }
        public string? NazivRanga { get; set; } = string.Empty;

    }
    public static partial class DtoMapping
    {
        public static Clan_RangZasluga ToAggregateDto3(this DomainModels.Clan clan)
            => new Clan_RangZasluga()
            {
                Id = clan.Id,
                Ime = clan.Ime,
                Prezime = clan.Prezime,
                RangId = clan.DodjeleZasluga?.MaxBy(d => d.Datum)?.RangZasluga.Id,
                NazivRanga = clan.DodjeleZasluga?.MaxBy(d => d.Datum)?.RangZasluga.Naziv,

                //DodjeleZasluga = clan.DodjeleZasluga == null
                  //              ? new List<DodjelaZasluga>()
                    //            : clan.DodjeleZasluga.Select(pr => pr.ToDto()).ToList(),
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

