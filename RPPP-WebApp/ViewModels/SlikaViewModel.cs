using RPPP_WebApp.Models;

namespace RPPP_WebApp.ViewModels;

public class SlikaViewModel
{
    public DateTime Datum { get; set; }
    public string Smjer { get; set; }
    public string Url { get; set; }
    public int KameraId { get; set; }
    public string KameraNaziv { get; set; }
    public virtual Kamere Kamera { get; set; }
}