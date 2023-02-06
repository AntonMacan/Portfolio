using System.ComponentModel.DataAnnotations;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{
    public partial class RangStarost
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name can't be empty", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Role name can't be longer than 50 characters")]
        public string Naziv { get; set; } =  string.Empty;

    }
    public static partial class DtoMapping
    {
        public static RangStarost ToDto(this DomainModels.RangStarost clan)
            => new RangStarost()
            {
                Id = clan.Id,
                Naziv = clan.Naziv,
            };

        public static DomainModels.RangStarost ToDomain(this RangStarost clan)
            => new DomainModels.RangStarost(
                clan.Id,
                clan.Naziv
            );

    }
}