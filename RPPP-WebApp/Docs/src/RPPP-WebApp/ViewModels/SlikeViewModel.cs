namespace RPPP_WebApp.ViewModels;

public class SlikeViewModel
{
    public IEnumerable<SlikaViewModel> Slike { get; set; }
    public KameraViewModel kamera { get; set; }
    public PagingInfo PagingInfo { get; set; }
}