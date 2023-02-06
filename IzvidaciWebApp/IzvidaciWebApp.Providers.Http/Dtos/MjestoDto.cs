using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers.Http.Dtos
{
    public class MjestoDto
    {
        public int PbrMjesta { get; set; }

        [Required(ErrorMessage = "Naziv mjesta ne smije biti null.")]
        [StringLength(50, ErrorMessage = "Naziv mjesta ne smije biti duzi od 50 znakova.")]
        public string NazivMjesta { get; set; } = string.Empty;
    }

    public static partial class DtoMapping
    {
        public static MjestoDto ToDto(this Mjesto mjesto)
        {
            return new MjestoDto()
            {
                PbrMjesta = mjesto.PbrMjesta,
                NazivMjesta = mjesto.NazivMjesta
            };
        }
        public static Mjesto ToDomain(this MjestoDto mjesto)
        {
            return new Mjesto(mjesto.PbrMjesta, mjesto.NazivMjesta);
        }

    }
}
