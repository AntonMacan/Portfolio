using RPPP_WebApp.Models;

namespace RPPP_WebApp.ViewModels;

public class KameraViewModel
{
    public int Id { get; set; }
    public string DionicaNaziv { get; set; }
    public int DionicaId { get; set; }
    public string Naziv { get; set; }
    public double? GeografskaSirina { get; set; }
    public double? GeografskaDuzina { get; set; }
    
    public virtual ICollection<SlikaViewModel> Slike { get; set; }
    
    public Func<double, string> KoordinateKonverter = UtilClasses.Converters.koordinateToWGS;
}