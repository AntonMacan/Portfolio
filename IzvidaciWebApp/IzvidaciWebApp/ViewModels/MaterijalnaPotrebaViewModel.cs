namespace IzvidaciWebApp.ViewModels
{
    public class MaterijalnaPotrebaViewModel
    {
        public int IdMaterijalnePotrebe { get; set; }
        public string Naziv { get; set; }
        public int Organizator { get; set; }

        public int Davatelj { get; set; }
        public bool Zadovoljeno { get; set; }



    }
}
