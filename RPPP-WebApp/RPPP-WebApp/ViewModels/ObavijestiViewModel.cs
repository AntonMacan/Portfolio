using RPPP_WebApp.Models;


namespace RPPP_WebApp.ViewModels
{
    public class ObavijestViewModel
    {
        public IEnumerable<Obavijesti> Obavijesti { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
