namespace RPPP_WebApp.ViewModels
{
    /// <summary>
    /// Model podataka potrebnog za prikaze jednog parkirališta gospodarskih vozila
    /// </summary>
    public class ParkiralisteGospVozViewModel
    {
        public int ParkingId { get; set; }
        public String Dionica { get; set; }
        public double Stacionaža { get; set; }
        public string Naziv { get; set; }
        public double GeoSirinaUlaz { get; set; }
        public double GeoDuzinaUlaz { get; set; }
        public int BrojMjesta { get; set; }
        public double? CijenaPoSatu { get; set; }
        public string StranaCesteUlaz { get; set; }
        
    }
}
