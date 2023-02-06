using RPPP_WebApp.Models;

namespace RPPP_WebApp.ViewModels
{

    /// <summary>
    /// Model za prikaz master-detail naplatne postaje i cjenika
    /// </summary>
    public class NaplatnaPostajaDetailView
    {
        public int NaplatnaID { get; set; }

        public string Ime { get; set; }

        public double GeoDuzina { get; set; }

        public double GeoSirina { get; set; }

        public IEnumerable<StavkaCjenikaViewModel> StavkeCjenika { get; set; }

        public Func<double, string> KoordinateKonverter = UtilClasses.Converters.koordinateToWGS;

        public Func<double?, string> CijenaFormater = UtilClasses.Converters.convertAndFormatPrice;
    }
}
