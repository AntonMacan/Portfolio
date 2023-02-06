using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.ViewModels
{
    public class OdmoristeViewModel
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public string Smjer { get; set; }
        public int? NadmorskaVisina { get; set; }
        public string Stacionaza { get; set; }
        public double? GeografskaSirina { get; set; }
        public double? GeografskaDuzina { get; set; }
        public string DionicaNaziv { get; set; }

        public Func<double, String> konverterKoordinata = Converters.koordinateToWGS;
        public virtual ICollection<Multimedija> Multimedija { get; set; }
        public virtual ICollection<Sadrzaj> Sadrzaj { get; set; }
    }
}
