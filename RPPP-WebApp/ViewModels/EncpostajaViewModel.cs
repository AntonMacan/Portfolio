namespace RPPP_WebApp.ViewModels
{

    /// <summary>
    /// Model za prikaz pojedinačne ENC postaje
    /// </summary>
    public class EncpostajaViewModel
    {
        public int Encid { get; set; }
        public TimeSpan VrijemeOtvaranja { get; set; }
        public TimeSpan VrijemeZatvaranja { get; set; }
        public string KontaktBroj { get; set; }
        public string Ime { get; set; }

        public string? Naplatna { get; set; }

        public int? NaplatnaStaza { get; set; }

    }
}
