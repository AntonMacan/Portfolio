using IzvidaciWebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers.Http.Dtos
{
    public class MaterijalnaPotrebaDto
    {


        public int IdMaterijalnaPotreba { get; set; }
        [Required]
        [StringLength(50)]
        public string Naziv { get; set; }
        public int Organizator { get; set; }
        public int Davatelj { get; set; }
        public bool Zadovoljeno { get; set; }

    }

    public static partial class DtoMapping
    {
        public static MaterijalnaPotrebaDto ToDto(this MaterijalnaPotreba materijalnaPotreba)
            => new MaterijalnaPotrebaDto()
            {
                IdMaterijalnaPotreba = materijalnaPotreba.IdMaterijalnaPotreba,
                Naziv = materijalnaPotreba.Naziv,
                Organizator = materijalnaPotreba.Organizator,
                Davatelj = materijalnaPotreba.Davatelj,
                Zadovoljeno = materijalnaPotreba.Zadovoljeno

            };

        public static MaterijalnaPotreba ToDomain(this MaterijalnaPotrebaDto materijalnaPotreba)
            => new MaterijalnaPotreba(
                materijalnaPotreba.IdMaterijalnaPotreba,
                materijalnaPotreba.Naziv,
                materijalnaPotreba.Organizator,
                materijalnaPotreba.Davatelj,
                materijalnaPotreba.Zadovoljeno);

    }
}
