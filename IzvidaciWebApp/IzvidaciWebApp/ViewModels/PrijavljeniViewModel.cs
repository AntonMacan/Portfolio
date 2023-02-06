namespace IzvidaciWebApp.ViewModels
{
    public class PrijavljeniViewModel
    {
        public int IdEdukacije { get; set; }
        public string NazivEdukacije { get; set; } = String.Empty;
        public IEnumerable<PrijavljenViewModel> prijavljeni { get; set; }
    }
}
