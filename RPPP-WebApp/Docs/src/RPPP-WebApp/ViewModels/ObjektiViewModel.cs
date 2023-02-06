using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.ViewModels;

public class ObjektiViewModel
{
    public IEnumerable<ObjektViewModel> Objekti { get; set; }
    public PagingInfo PagingInfo { get; set; }
    public Func<double, String> konverterKoordinata = Converters.koordinateToWGS;
}