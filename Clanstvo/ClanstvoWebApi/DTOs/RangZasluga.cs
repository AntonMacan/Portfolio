using System.ComponentModel.DataAnnotations;
using DomainModels = Clanstvo.Domain.Models;

namespace ClanstvoWebApi.DTOs
{
    public partial class RangZasluga
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name can't be empty", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Role name can't be longer than 50 characters")]
        public string Naziv { get; set; } = string.Empty;

    }
    public static partial class DtoMapping
    {
        public static RangZasluga ToDto(this DomainModels.RangZasluga clan)
            => new RangZasluga()
            {
                Id = clan.Id,
                Naziv = clan.Naziv,
            };

        public static DomainModels.RangZasluga ToDomain(this RangZasluga clan
            )
            => new DomainModels.RangZasluga(
                clan.Id,
                clan.Naziv
            );
    }
}
