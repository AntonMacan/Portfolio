using RPPP_WebApp.UtilClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPPP_WebApp.ModelsPartials
{

    /// <summary>
    /// Denormalizirana stavka cjenika potrebna za PDF dokument
    /// </summary>
    public class StavkaCjenikaDenorm
    {
        public int NaplatnaID { get; set; }

        public string Ime { get; set; }

        public double GeoDuzina { get; set; }

        public double GeoSirina { get; set; }

        public int IzlazId { get; set; }
        public string ImeIzlaza { get; set; }

        public double GeoDuzinaIzlaza { get; set; }
        public double GeoSirinaIzlaza { get; set; }
        public int CijenaIa { get; set; }
        public int CijenaI { get; set; }
        public int CijenaIi { get; set; }
        public int CijenaIii { get; set; }
        public int CijenaIv { get; set; }

        
    }
}
