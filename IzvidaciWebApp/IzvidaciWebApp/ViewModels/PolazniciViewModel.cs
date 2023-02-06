namespace IzvidaciWebApp.ViewModels
{
    public class PolazniciViewModel
    {
        public int IdEdukacije { get; set; }
        public string NazivEdukacije { get; set; } = String.Empty;
        public IEnumerable<PolaznikViewModel> polaznici { get; set; }
    }
}
