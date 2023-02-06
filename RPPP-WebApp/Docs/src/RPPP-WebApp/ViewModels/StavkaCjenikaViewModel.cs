using System.ComponentModel.DataAnnotations;

namespace RPPP_WebApp.ViewModels
{
    /// <summary>
    /// Predstavlja model stavke cjenika koji služi za pogled 
    /// </summary>
    public class StavkaCjenikaViewModel
    {
        public int IzlazID { get; set; }
        public string Ime { get; set; }

        public double GeoDuzina { get; set; }

        public double GeoSirina { get; set; }

        [Display(Name = "Cijena kategoriju IA")]
        public int cijenaIA { get; set; }

        [Display(Name = "Cijena za kategoriju I")]

        public int cijenaI { get; set; }

        [Display(Name = "Cijena kategorije II")]

        public int cijenaII { get; set; }

        [Display(Name = "Cijena kategorije III")]

        public int cijenaIII { get; set; }

        [Display(Name = "Cijena kategorije IV")]

        public int cijenaIV { get; set; }

        public Func<double?, string> formatCijena = UtilClasses.Converters.convertAndFormatPrice;
        public Func<double, string> KoordinateKonverter = UtilClasses.Converters.koordinateToWGS;

    }
}
