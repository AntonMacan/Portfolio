using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;


namespace RPPP_WebApp.ViewModels
{

    /// <summary>
    /// Model više naplatnih postaja
    /// </summary>
    public class NaplatnePostajeViewModel 
    {
        public IEnumerable<NaplatnaPostajaViewModel> NaplatnePostaje { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public Func<double, String> KoordinateFormater = Converters.koordinateToWGS;
        public Func<TimeSpan?, TimeSpan?, String> RadnoVrijemeFormater = Converters.RadnoVrijemeFormater;


    }
}
