using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.ViewModels
{
    public class AkcijaViewModel 
    {
        public int IdAkcije { get; set; }
        public string Naziv { get; set; } 
        public int MjestoPbr { get; set; }
        public int Organizator { get; set; }
        public int KontaktOsoba { get; set; }

        public string? Vrsta { get; set; }
    }
}
