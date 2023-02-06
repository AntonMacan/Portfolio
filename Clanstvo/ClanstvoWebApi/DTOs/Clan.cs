using System.ComponentModel.DataAnnotations;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{


    public partial class Clan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name can't be null")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
        public string Ime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name can't be null")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
        public string Prezime { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime DatumRodenja { get; set; }
        public byte[]? Slika { get; set; }

        [Required(ErrorMessage = "Address can't be null")]
        [StringLength(50, ErrorMessage = "Address can't be longer than 50 characters")]
        public string Adresa { get; set; } = string.Empty;
        public bool ImaMaramu { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DatumMarama { get; set; }

        [StringLength(50)]
        public string? MjestoMarama { get; set; } = string.Empty;
    }
    public static partial class DtoMapping
    {
        public static Clan ToDto(this DomainModels.Clan clan)
            => new Clan()
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
            };

        public static DomainModels.Clan ToDomain(this Clan clan)
            => new DomainModels.Clan(
                clan.Id,
                clan.Ime,
                clan.Prezime,
                clan.DatumRodenja,
                clan.Slika,
                clan.Adresa,
                clan.ImaMaramu,
                clan.DatumMarama,
                clan.MjestoMarama
            );

    }
}

