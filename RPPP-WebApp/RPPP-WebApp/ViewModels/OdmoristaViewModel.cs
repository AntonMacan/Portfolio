using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.ViewModels
{
    public class OdmoristaViewModel
    {
        public IEnumerable<OdmoristeViewModel> Odmorista { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public Func<double, String> konverterKoordinata = Converters.koordinateToWGS;
    }
}
