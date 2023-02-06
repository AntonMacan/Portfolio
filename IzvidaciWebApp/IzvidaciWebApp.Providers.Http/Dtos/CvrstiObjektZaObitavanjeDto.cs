using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos
{
    public class CvrstiObjektZaObitavanjeDto
    {
        public int IdTerenskaLokacija { get; set; }
        public string NazivTerenskaLokacija { get; set; }
        public byte[] Slika { get; set; }
        public bool ImaSanitarniCvor { get; set; }
        public int MjestoPbr { get; set; }
        public string Opis { get; set; }
        public int BrojPredvidenihSpavacihMjesta { get; set; }
    }
    
    public static partial class DtoMapping
    {
        public static CvrstiObjektZaObitavanjeDto ToDto(this CvrstiObjektZaObitavanje cvrstiObjektZaObitavanje)
            => new CvrstiObjektZaObitavanjeDto()
            {
                IdTerenskaLokacija = cvrstiObjektZaObitavanje.IdTerenskaLokacija,
                NazivTerenskaLokacija = cvrstiObjektZaObitavanje.NazivTerenskaLokacija,
                Slika = cvrstiObjektZaObitavanje.Slika,
                ImaSanitarniCvor = cvrstiObjektZaObitavanje.ImaSanitarniCvor,
                MjestoPbr = cvrstiObjektZaObitavanje.MjestoPbr,
                Opis = cvrstiObjektZaObitavanje.Opis,
                BrojPredvidenihSpavacihMjesta = cvrstiObjektZaObitavanje.BrojPredvidenihSpavacihMjesta
            };

        public static CvrstiObjektZaObitavanje ToDomain(this CvrstiObjektZaObitavanjeDto cvrstiObjektZaObitavanje)
            => new CvrstiObjektZaObitavanje(
                cvrstiObjektZaObitavanje.IdTerenskaLokacija,
                cvrstiObjektZaObitavanje.NazivTerenskaLokacija,
                cvrstiObjektZaObitavanje.Slika,
                cvrstiObjektZaObitavanje.ImaSanitarniCvor,
                cvrstiObjektZaObitavanje.MjestoPbr,
                cvrstiObjektZaObitavanje.Opis,
                cvrstiObjektZaObitavanje.BrojPredvidenihSpavacihMjesta
                );
    }
}