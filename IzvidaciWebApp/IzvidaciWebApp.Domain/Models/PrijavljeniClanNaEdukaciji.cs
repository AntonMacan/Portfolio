namespace IzvidaciWebApp.Domain.Models;

public class PrijavljeniClanNaEdukaciji
{
    private int _idPolaznik;
    private DateTime _datumPrijave;

    public PrijavljeniClanNaEdukaciji(int idPolaznik, DateTime datumPrijave)
    {
        _idPolaznik = idPolaznik;
        _datumPrijave = datumPrijave;
    }

    public int idPolaznik { get => _idPolaznik; set => _idPolaznik = value; }
    public DateTime datumPrijave { get => _datumPrijave; set => _datumPrijave = value; }
}

