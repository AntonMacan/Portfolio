using RPPP_WebApp.Models;

namespace RPPP_WebApp.ViewModels;

public class ObjektViewModel
{
    public int Id { get; set; }
    public string DionicaNaziv { get; set; }
    public int DionicaID { get; set; }
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public string TipObjekta { get; set; }
    public int? NadmorskaVisina { get; set; }
    public int? Stacionaza { get; set; }
    public int? DimenzijeM { get; set; }
    public double? GeografskaSirina { get; set; }
    public double? GeografskaDuzina { get; set; }

    public virtual ICollection<OdrzavanjeObjektaViewModel> OdrzavanjeObjekta { get; set; }
    public Func<double, string> KoordinateKonverter = UtilClasses.Converters.koordinateToWGS;

}