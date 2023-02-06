namespace IzvidaciWebApp.ViewModels;

public class ProstorViewModel
{
    public int id { get; set; }
    public int idUdruge { get; set; }
    public string adresa { get; set; }
    public string namjena { get; set; }
    public string dodijelio { get; set; }
    public DateTime? dodjeljenoDo { get; set; }
    public decimal? geoDuzina { get; set; }
    public decimal? geoSirina { get; set; }
    
}
