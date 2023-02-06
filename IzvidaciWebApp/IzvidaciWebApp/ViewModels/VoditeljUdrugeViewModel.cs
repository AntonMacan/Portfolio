namespace IzvidaciWebApp.ViewModels
{
    public class VoditeljUdrugeViewModel
    {

        public int IdUdruge { get; set; }
        public int IdClan { get; set; } 
        public string Pozicija { get; set; } = String.Empty;
        public DateTime? NaPozicijiDo { get; set; } 

    }
}