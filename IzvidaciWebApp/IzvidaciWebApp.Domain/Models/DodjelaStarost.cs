namespace IzvidaciWebApp.Domain.Models;

public class DodjelaStarost
{
    private readonly DateTime _datum;
    private readonly RangStarost _rang;

    public DodjelaStarost(DateTime datum, RangStarost rang)
    {
        _datum = datum;
        _rang = rang;
    }

    public DateTime Datum => _datum;                                                                                                                                                                
    public RangStarost RangStarost => _rang;
}