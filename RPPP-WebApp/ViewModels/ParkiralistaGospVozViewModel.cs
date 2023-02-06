using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.ViewModels
{

    /// <summary>
    /// Model više parkirališta 
    /// </summary>
    public class ParkiralistaGospVozViewModel
    {
        public IEnumerable<ParkiralisteGospVozViewModel> parkiralista { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public Func<double, string> koordinateKonverter = Converters.koordinateToWGS;
        public Func<double?, string> cijenaKonverter = Converters.convertAndFormatPrice;
    }
}
