using System.ComponentModel.DataAnnotations;
using System.Data;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{
    public partial class DodjelaZasluga
    {
        [Required(ErrorMessage = "Role to assign must be provided")]
        public RangZasluga RangZasluga { get; set; }
        [Required(ErrorMessage = "Date when the role was given on must be provided")]
        public DateTime Datum { get; set; }

    }
    public static partial class DtoMapping
    {
        public static DodjelaZasluga ToDto(this DomainModels.DodjelaZasluga dodjelaZasluga)
            => new DodjelaZasluga()
            {
                Datum = dodjelaZasluga.Datum,
                RangZasluga = dodjelaZasluga.RangZasluga.ToDto()
            };

        public static DomainModels.DodjelaZasluga ToDomain(this DodjelaZasluga dodjelaZasluga, int clanId)
            => new DomainModels.DodjelaZasluga(
                dodjelaZasluga.Datum,
                dodjelaZasluga.RangZasluga.ToDomain()
            );

    }
}
