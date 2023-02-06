namespace IzvidaciWebApp.ViewModels;

public class RangZaslugeViewModel
{
    public IEnumerable<RangZaslugaViewModel> rangovi { get; set; }
    public int sort { get; set; }
    public bool ascending { get; set; }
}