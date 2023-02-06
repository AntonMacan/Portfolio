using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.ViewModels;

public class KamereViewModel
{
    
    public IEnumerable<KameraViewModel> Kamere { get; set; }
    public PagingInfo PagingInfo { get; set; }
    public Func<double, String> konverterKoordinata = Converters.koordinateToWGS;
}