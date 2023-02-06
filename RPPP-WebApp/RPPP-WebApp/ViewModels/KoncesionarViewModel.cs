using RPPP_WebApp.Models;


namespace RPPP_WebApp.ViewModels
{
    public class KoncesionarViewModel
    {
        public IEnumerable<Koncesionari> Koncesionari { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
