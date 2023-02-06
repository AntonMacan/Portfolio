namespace IzvidaciWebApp.Domain.Models;

public class Prostori{
    private int _Id;
    private int _IdUdruge;
    private string _Adresa;
    private string _Namjena;
    private string _Dodijelio;
    private DateTime? _DodjeljenoDo;
    private decimal? _GeoDuzina;
    private decimal? _GeoSirina;

    public Prostori(int id, int idUdruge, string adresa, string namjena, string dodijelio, DateTime? dodjeljenoDo, decimal? geoDuzina, decimal? geoSirina)
    {
        _Id = id;
        _IdUdruge = idUdruge;
        _Adresa = adresa;
        _Namjena = namjena;
        _Dodijelio = dodijelio;
        _DodjeljenoDo = dodjeljenoDo;
        _GeoDuzina = geoDuzina;
        _GeoSirina = geoSirina;
    }

    public int Id => _Id;
    public int IdUdruge => _IdUdruge;
    public string Adresa => _Adresa;
    public string Namjena => _Namjena;
    public string Dodijelio => _Dodijelio;
    public DateTime? DodjeljenoDo => _DodjeljenoDo;
    public decimal? GeoDuzina => _GeoDuzina;
    public decimal? GeoSirina => _GeoSirina;
    
}