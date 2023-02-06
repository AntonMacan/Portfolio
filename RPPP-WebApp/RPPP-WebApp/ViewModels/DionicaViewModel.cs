using RPPP_WebApp.Models;


namespace RPPP_WebApp.ViewModels
{
    public class DionicaViewModel
    {
        public IEnumerable<Dionice> Dionice { get; set; }
        
        public PagingInfo PagingInfo { get; set; }
    }
}