namespace IzvidaciWebApp.ViewModels
{
    public class EdukacijeViewModel
    {
        public int IdSkole { get; set; }
        public string NazivSkole { get; set; } = String.Empty;
        public IEnumerable<EdukacijaViewModel> edukacije { get; set; }
    }
}
