using RPPP_WebApp.Models;

namespace RPPP_WebApp.ViewModels;

public class OdrzavanjaObjektaViewModel
{
    public  IEnumerable<OdrzavanjeObjektaViewModel> OdrzavanjeObjekta { get; set; }

    public ObjektViewModel objekt { get; set; }
    public PagingInfo PagingInfo { get; set; }
}