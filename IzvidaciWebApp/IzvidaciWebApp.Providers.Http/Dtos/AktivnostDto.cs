using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos
{
    public class AktivnostDto
    {
        public int IdAktivnost { get; set; }
        public int MjestoPbr { get; set; }
        public int KontaktOsoba { get; set; }
        public string Opis { get; set; } = string.Empty;
        public int AkcijaId { get; set; }
    }

    public static partial class DtoMapping
    {
        public static AktivnostDto ToDto(this IzvidaciWebApp.Domain.Models.Aktivnost aktivnost)
        {
            return new AktivnostDto()
            { 
                IdAktivnost = aktivnost.Id,
                MjestoPbr = aktivnost.MjestoPbr,
                KontaktOsoba = aktivnost.KontaktOsoba,
                Opis = aktivnost.Opis,
                AkcijaId = aktivnost.AkcijaId
            };
        }
        public static IzvidaciWebApp.Domain.Models.Aktivnost ToDomain(this AktivnostDto aktivnost)
        {
            return new IzvidaciWebApp.Domain.Models.Aktivnost(aktivnost.IdAktivnost, aktivnost.MjestoPbr, aktivnost.KontaktOsoba, aktivnost.Opis, aktivnost.AkcijaId);
        }

    }
}
