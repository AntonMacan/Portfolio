using System.Text;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.UtilClasses
{
    /// <summary>
    /// Razred s metodama za formatiranje i konverziju podataka
    /// </summary>
    public class Converters 
    {
        /// <summary>
        /// Pretvara geografkse koordinate u decimalnom obliku u WGS84 oblik(° ' '')
        /// </summary>
        /// <param name="koordinate">Geografske koordinate u deicmalnom obliku</param>
        /// <returns>string oblika xx°yy'zz''</returns>
        public static String koordinateToWGS(double koordinate)
        {
            String wgs ="";

            
            int sec = (int)Math.Round(koordinate * 3600);
            int deg = sec / 3600;
            sec = Math.Abs(sec % 3600);
            int min = sec / 60;
            sec %= 60;

            String stupnjevi = "";
            String minute = "";
            String sekunde = "";

            if (sec < 10) sekunde += "0" + sec;
            else { sekunde += sec; }

            if (min < 10) minute += "0" + min;
            else { minute += min; }

            if (deg < 10) stupnjevi += "0" + deg;
            else { stupnjevi += deg;}




            wgs = stupnjevi + "° " + minute + "' " + sekunde + "''";

            return wgs;
        }

        /// <summary>
        /// Konvertira i formatira cijenu u kunama u format xx.yy kn (€xx.yy)
        /// </summary>
        /// <param name="cijena">Cijena u kunama</param>
        /// <returns>string oblika xx.yy kn (€xx.yy)</returns>
        public static String convertAndFormatPrice(double? cijena)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("{0:0.##}", cijena));
            sb.Append(" kn (€");
            sb.Append(String.Format("{0:0.##}", cijena / 7.5));
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Stvara ime dionice iz naziva njezine ulazne i izlazne naplatne postaje
        /// </summary>
        /// <param name="ulaz">Naziv ulazne naplatne postaje</param>
        /// <param name="izlaz">Naziv izlazne naplazne postaja</param>
        /// <param name="oznakaAutoceste">Oznaka autoceste na kojoj se nalazi</param>
        /// <returns>string oblika nazivUlaz-nazivIzlaz</returns>
        public static String GetDionicaName(string ulaz, string izlaz, string oznakaAutoceste)
        {
            StringBuilder dionica = new StringBuilder(ulaz.Substring(ulaz.IndexOf(oznakaAutoceste) + oznakaAutoceste.Length + 1));
            dionica.Append(" - ");
            dionica.Append(izlaz.Substring(izlaz.IndexOf(oznakaAutoceste) + oznakaAutoceste.Length + 1));


            return dionica.ToString();
        }

        /// <summary>
        /// Stvara ime dionice 
        /// Delegira izvođenje općenitijoj metodi
        /// </summary>
        /// <param name="d">Objekt Dionica</param>
        /// <returns>string oblika nazivUlaz-nazivIzlaz</returns>
        public static String GetDionicaName(Dionice d)
        {
            if (d == null)
                return "-";

            return GetDionicaName(d.UlaznaPostajaNavigation.Ime, d.IzlaznaPostajaNavigation.Ime, d.OznakaAutoceste);
        }


        /// <summary>
        /// Formatira radno vrijeme u hh:mm format
        /// </summary>
        /// <param name="VrijemeOtvaranja">vrijeme otvaranja</param>
        /// <param name="VrijemeZatvaranja">vrijeme zatvaranja</param>
        /// <returns>string oblika hh:mm</returns>
        public static String RadnoVrijemeFormater(TimeSpan? VrijemeOtvaranja, TimeSpan? VrijemeZatvaranja)
        {
            string otvara = $@"{VrijemeOtvaranja:hh\:mm}";
            string zatvara = $@"{VrijemeZatvaranja:hh\:mm}";

            return otvara + "-" + zatvara;
        }
    }
}
