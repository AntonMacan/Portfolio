using System.ComponentModel.DataAnnotations;

using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{
    public class ClanAggregate
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "First name can't be empty", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
        public string Ime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name can't be empty", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
        public string Prezime { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime DatumRodenja { get; set; }
        public byte[] Slika { get; set; }

        [Required(ErrorMessage = "Address can't be empty", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Address can't be longer than 50 characters")]
        public string Adresa { get; set; } = string.Empty;
        public bool ImaMaramu { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DatumMarama { get; set; }
        [StringLength(50)]

        public string MjestoMarama { get; set; } = string.Empty;


        public IEnumerable<DodjelaStarost> DodjeleStarost { get; set; } = Enumerable.Empty<DodjelaStarost>();
        
        public IEnumerable<DodjelaZasluga> DodjeleZasluga { get; set; } = Enumerable.Empty<DodjelaZasluga>();

        public IEnumerable<Clanarina> Clanarina { get; set; } = Enumerable.Empty<Clanarina>();
    }
    public static partial class DtoMapping
    {
        public static ClanAggregate ToAggregateDto(this DomainModels.Clan clan)
            => new ClanAggregate()
            {
                Id = clan.Id,
                Ime = clan.Ime,
                Prezime = clan.Prezime,
                Slika = clan.Slika,
                DatumRodenja = clan.DatumRodenja,
                Adresa = clan.Adresa,
                ImaMaramu = clan.ImaMaramu,
                DatumMarama = clan.DatumMarama,
                MjestoMarama = clan.MjestoMarama,
                DodjeleStarost = clan.DodjeleStarost == null
                                ? new List<DodjelaStarost>()
                                : clan.DodjeleStarost.Select(pr => pr.ToDto()).ToList(),
                DodjeleZasluga = clan.DodjeleZasluga == null
                                ? new List<DodjelaZasluga>()
                                : clan.DodjeleZasluga.Select(pr => pr.ToDto()).ToList(),
                Clanarina = clan.Clanarina == null
                                ? new List<Clanarina>()
                                : clan.Clanarina.Select(pr => pr.ToDto()).ToList()
            };

        public static DomainModels.Clan ToDbModel(ClanAggregate clan)
            => new DomainModels.Clan(
                clan.Id,
                clan.Ime,
                clan.Prezime,
                clan.DatumRodenja,
                clan.Slika,
                clan.Adresa,
                clan.ImaMaramu,
                clan.DatumMarama,
                clan.MjestoMarama,
                clan.DodjeleStarost.Select(ToDomain),
                clan.DodjeleZasluga.Select(ToDomain),
                clan.Clanarina.Select(ToDomain)
            );
       
    }
}
