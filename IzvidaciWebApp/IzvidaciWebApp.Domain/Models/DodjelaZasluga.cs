namespace IzvidaciWebApp.Domain.Models;
public class DodjelaZasluga
{
    private readonly DateTime _datum;
    private readonly RangZasluga _rang;

    public DodjelaZasluga(DateTime datum, RangZasluga rang)
    {
        _datum = datum;
        _rang = rang;
    }

    public DateTime Datum => _datum;
    public RangZasluga RangZasluga => _rang;
}