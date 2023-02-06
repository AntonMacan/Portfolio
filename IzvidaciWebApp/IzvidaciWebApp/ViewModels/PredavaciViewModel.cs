namespace IzvidaciWebApp.ViewModels
{
    public class PredavaciViewModel
    {
        public int IdEdukacije { get; set; }
        public string NazivEdukacije { get; set; } = String.Empty;
        public IEnumerable<PredavacViewModel> predavaci { get; set; }
    }
}
