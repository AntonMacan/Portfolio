using RPPP_WebApp.Models;


namespace RPPP_WebApp.ViewModels
{
    public class AutocestaViewModel
    {
        public IEnumerable<Autoceste> Autoceste { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
