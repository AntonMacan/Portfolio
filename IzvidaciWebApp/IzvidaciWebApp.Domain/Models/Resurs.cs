namespace IzvidaciWebApp.Domain.Models;

public class Resurs
{
    private readonly int _id;
    private string _naziv;
    private string? _napomena;
    private DateTime _datumNabave;
    private int _idUdruge;
    private string _udruga;
    private int _idProstor;
    private string _prostor;

    public Resurs(string naziv, string? napomena, DateTime datumNabave, int idUdruge, string udruga, int idProstor, string prostor, int id)
    {
        _naziv = naziv;
        _napomena = napomena;
        _datumNabave = datumNabave;
        _idUdruge = idUdruge;
        _idProstor = idProstor;
        _id = id;
        _udruga = udruga;
        _prostor = prostor;
    }

    public int Id => _id;
    public string Naziv { get => _naziv; set => _naziv = value; }
    public string Napomena { get => _napomena; set => _napomena = value; }
    public DateTime DatumNabave { get => _datumNabave; set => _datumNabave = value; }
    public int IdUdruge { get => _idUdruge; set => _idUdruge = value; }
    public string Udruga { get => _udruga; set => _udruga = value; }
    public int IdProstor { get => _idProstor; set => _idProstor = value; }
    public string Prostor { get => _prostor; set => _prostor = value; }
}