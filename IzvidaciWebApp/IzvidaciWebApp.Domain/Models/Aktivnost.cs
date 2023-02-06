using BaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Domain.Models
{
    public class Aktivnost
    {
        private int _Id;
        private int _MjestoPbr;
        private int _KontaktOsoba;
        private string _Opis;
        private int _AkcijaId;


        public Aktivnost(int id, int mjestoPbr, int kontaktOsoba, string opis, int akcijaId)
        {
            _Id = id;
            _MjestoPbr = mjestoPbr;
            _Opis = opis;
            _KontaktOsoba = kontaktOsoba;
            _AkcijaId = akcijaId;
        }

        public int Id { get => _Id; set => _Id = value; }
        public int MjestoPbr { get => _MjestoPbr; set => _MjestoPbr = value; }
        public int KontaktOsoba { get => _KontaktOsoba; set => _KontaktOsoba = value; }
        public string Opis { get => _Opis; set => _Opis = value; }
        public int AkcijaId { get => _AkcijaId; set => _AkcijaId = value; }

        public override bool Equals(object? obj)
        {
            return obj is not null &&
                   obj is Aktivnost aktivnost &&
                   Id.Equals(aktivnost.Id) &&
                   MjestoPbr.Equals(aktivnost.MjestoPbr) &&
                   MjestoPbr.Equals(aktivnost.KontaktOsoba) &&
                   Opis.Equals(aktivnost.Opis) &&
                   AkcijaId.Equals(aktivnost.AkcijaId);

        }
       
    }
}
