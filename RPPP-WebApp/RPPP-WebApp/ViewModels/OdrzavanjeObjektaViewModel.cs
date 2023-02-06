using RPPP_WebApp.Models;

namespace RPPP_WebApp.ViewModels;

public class OdrzavanjeObjektaViewModel
{
    public int Id { get; set; }
    public string NazivObjekta { get; set; }
    public int ObjektId { get; set; }
    public DateTime datum { get; set; }
    public String Opis { get; set; } 
    public string Ishod { get; set; }
    public string TipNaziv { get; set; }
    public int TipId { get; set; }
    public string Odrzavatelj { get; set; }
    
    public virtual ICollection<TipOdrzavanja> TipOdrzavanja { get; set; }
}