namespace IzvidaciWebApp.Domain.Models;

public class TrajniResurs : Resurs
{

    private string _inventarniBroj;
    private bool _jeDostupno;
    
    public string InventarniBroj
    {
        get => _inventarniBroj;
        set => _inventarniBroj = value;
    }

    public bool JeDostupno
    {
        get => _jeDostupno;
        set => _jeDostupno = value;
    }
    
    public TrajniResurs(int id, string naziv, string? napomena, DateTime datumNabave, int idUdruge,  string udruga, int idProstor, string prostor, string inventarniBroj, bool jeDostupno) : base(naziv, napomena, datumNabave, idUdruge, udruga, idProstor, prostor, id)
    {
        _inventarniBroj = inventarniBroj;
        _jeDostupno = jeDostupno;
    }

}