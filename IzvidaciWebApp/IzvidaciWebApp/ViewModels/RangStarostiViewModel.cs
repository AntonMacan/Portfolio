namespace IzvidaciWebApp.ViewModels;

public class RangStarostiViewModel
{
    public IEnumerable<RangStarostViewModel> rangovi { get; set; }
    public int sort { get; set; }
    public bool ascending { get; set; }
}