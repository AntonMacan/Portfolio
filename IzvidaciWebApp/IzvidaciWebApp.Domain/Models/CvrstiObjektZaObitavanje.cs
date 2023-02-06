using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Domain.Models
{
    public class CvrstiObjektZaObitavanje
    {
        private int _IdTerenskaLokacija;
        private string _NazivTerenskaLokacija;
        private byte[] _Slika;
        private bool _ImaSanitarniCvor;
        private int _MjestoPbr;
        private string _Opis;
        private int _BrojPredvidenihSpavacihMjesta;

        public CvrstiObjektZaObitavanje(int id, string naziv, byte[] slika, bool imaSanitarniCvor, int mjestoPbr, string opis, int brojPredvidenihSpavacihMjesta)
        {
            _IdTerenskaLokacija = id;
            _NazivTerenskaLokacija = naziv;
            _Slika = slika;
            _ImaSanitarniCvor = imaSanitarniCvor;
            _MjestoPbr = mjestoPbr;
            _Opis = opis;
            _BrojPredvidenihSpavacihMjesta = brojPredvidenihSpavacihMjesta;
        }

        public int IdTerenskaLokacija { get => _IdTerenskaLokacija; set => _IdTerenskaLokacija = value; }
        public string NazivTerenskaLokacija { get => _NazivTerenskaLokacija; set => _NazivTerenskaLokacija = value; }
        public byte[] Slika { get => _Slika; set => _Slika = value; }
        public bool ImaSanitarniCvor { get => _ImaSanitarniCvor; set => _ImaSanitarniCvor = value; }
        public int MjestoPbr { get => _MjestoPbr; set => _MjestoPbr = value; }
        public string Opis { get => _Opis; set => _Opis = value; }
        public int BrojPredvidenihSpavacihMjesta { get => _BrojPredvidenihSpavacihMjesta; set => _BrojPredvidenihSpavacihMjesta = value; }

        public override bool Equals(object? obj)
        {
            return obj is not null &&
                obj is CvrstiObjektZaObitavanje lokacija &&
                IdTerenskaLokacija.Equals(lokacija.IdTerenskaLokacija) &&
                NazivTerenskaLokacija.Equals(lokacija.NazivTerenskaLokacija) &&
                Slika.Equals(lokacija.Slika) &&
                ImaSanitarniCvor.Equals(lokacija.ImaSanitarniCvor) &&
                MjestoPbr.Equals(lokacija.MjestoPbr) &&
                Opis.Equals(lokacija.Opis) &&
                BrojPredvidenihSpavacihMjesta.Equals(lokacija.BrojPredvidenihSpavacihMjesta);
        }
    }
}