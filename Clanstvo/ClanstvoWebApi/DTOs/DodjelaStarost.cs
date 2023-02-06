using System.ComponentModel.DataAnnotations;
using System.Data;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{
    public partial class DodjelaStarost
    {
        [Required(ErrorMessage = "Role to assign must be provided")]
        public RangStarost RangStarost { get; set; }
        [Required(ErrorMessage = "Date when the role was given on must be provided")]
        public DateTime Datum { get; set; }
        
    }
    public static partial class DtoMapping
    {
        public static DodjelaStarost ToDto(this DomainModels.DodjelaStarost dodjelaStarost)
            => new DodjelaStarost()
            {
                Datum = dodjelaStarost.Datum,
                RangStarost = dodjelaStarost.RangStarost.ToDto()
            };

        public static DomainModels.DodjelaStarost ToDomain(this DodjelaStarost dodjelaStarost, int clanId)
            => new DomainModels.DodjelaStarost(
                dodjelaStarost.Datum,
                dodjelaStarost.RangStarost.ToDomain()
            );

    }
}
