using System.ComponentModel.DataAnnotations.Schema;

namespace RPPP_WebApp.Models
{

    /// <summary>
    /// Model ENC postaje koja je vezana uz naplatnu postaju
    /// </summary>
    public class ViewNaplatnaPostajaENC
    {
        public int NaplatnaId { get; set; }
        public string Ime { get; set; }

        public double GeoDuzina { get; set; }

        public double GeoSirina { get; set; }

        public string? KontaktBroj { get; set; }

        public int? naplatnaStaza { get; set; }

        public TimeSpan? VrijemeOtvaranja { get; set; }
        public TimeSpan? VrijemeZatvaranja { get; set; }

        [NotMapped]
        public int Position { get; set; }
    }
}
