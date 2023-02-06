using System.ComponentModel.DataAnnotations;

namespace IzvidaciWebApp.ViewModels
{
    public class SkolaViewModel
    {
        public int Id { get; set; }
        public string NazivSkole { get; set; } = String.Empty;
        public int MjestoPbr { get; set; }
        public String NazivMjesto { get; set; } = string.Empty;
        public int Organizator { get; set; }
        public int KontaktOsoba { get; set; }
        public String ImeOrganizator { get; set; } = string.Empty;
        public String PrezimeOrganizator { get; set; } = string.Empty;
        public String ImeKontakt { get; set; } = string.Empty;
        public String PrezimeKontakt { get; set; } = string.Empty;
    }
}
