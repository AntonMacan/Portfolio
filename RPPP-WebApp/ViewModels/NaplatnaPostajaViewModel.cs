using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.ViewModels
{

    /// <summary>
    /// Model naplatne postaje s ENC postajom i izlaznim postajama
    /// </summary>
    public class NaplatnaPostajaViewModel : ViewNaplatnaPostajaENC
    {
        public ICollection<NaplatnaPostaja> IzlazneNaplatnePostaje { get; set; }
       
    }
}
