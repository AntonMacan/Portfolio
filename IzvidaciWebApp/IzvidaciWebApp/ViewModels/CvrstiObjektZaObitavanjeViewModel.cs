namespace IzvidaciWebApp.ViewModels
{
    public class CvrstiObjektZaObitavanjeViewModel
    {
        public int IdTerenskaLokacija { get; set; }
        public string NazivTerenskaLokacija { get; set; }
        public byte[] Slika { get; set; }
        public bool ImaSanitarniCvor { get; set; }
        public int MjestoPbr { get; set; }
        public string Opis { get; set; }
        public int BrojPredvidenihSpavacihMjesta { get; set; }
    }
}