namespace IzvidaciWebApp.Domain.Models;
public class PolaznikNaEdukaciji
{
    private int _idPolaznik;

    public PolaznikNaEdukaciji(int idPolaznik)
    {
        _idPolaznik = idPolaznik;
    }

    public int idPolaznik { get => _idPolaznik; set => _idPolaznik = value; }

}
