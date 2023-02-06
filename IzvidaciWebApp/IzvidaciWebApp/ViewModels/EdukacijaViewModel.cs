namespace IzvidaciWebApp.ViewModels
{
    public class EdukacijaViewModel
    {
        public int Id { get; set; }
        public string NazivEdukacije { get; set; } = String.Empty;
        public int MjestoPbr { get; set; }
        public string Opis { get; set; }
        public int IdSkole { get; set; }
    }
}
