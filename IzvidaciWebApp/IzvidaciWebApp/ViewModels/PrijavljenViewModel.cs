using Microsoft.Extensions.Caching.Memory;

namespace IzvidaciWebApp.ViewModels
{
    public class PrijavljenViewModel
    {
        public int IdClan { get; set; }
        public DateTime Datum { get; set; }
        public String Ime { get; set; } = string.Empty;
        public String Prezime { get; set; } = string.Empty;
    }
}
