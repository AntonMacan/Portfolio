using IzvidaciWebApp.Domain.Models;
using System;
public class Mjesto
{
    private int _Pbr;
    private string _NazivMjesta;
    private List<Akcija> _akcijeMjesta;
    private List<Aktivnost> _aktivnostiMjesta;

    public Mjesto(int id, string naziv,IEnumerable<Akcija>? aktivnostiAkcije = null, List<Aktivnost> aktivnostiMjesta = null)
    {
        _Pbr = id;
        _NazivMjesta = naziv;
        _akcijeMjesta = aktivnostiAkcije?.ToList() ?? new List<Akcija>();
        _aktivnostiMjesta = aktivnostiMjesta;
    }

    public int PbrMjesta { get => _Pbr; set => _Pbr = value; }
    public string NazivMjesta { get => _NazivMjesta; set => _NazivMjesta = value; }

    public IReadOnlyList<Akcija> akcijeMjesta => _akcijeMjesta.ToList();
    public IReadOnlyList<Aktivnost> aktivnostiMjesta => _aktivnostiMjesta.ToList();

    public override bool Equals(object? obj)
    {
        return obj is not null &&
            obj is Mjesto mjesto &&
            PbrMjesta.Equals(mjesto.PbrMjesta) &&
            NazivMjesta.Equals(mjesto.NazivMjesta) ;
        akcijeMjesta.SequenceEqual(mjesto.akcijeMjesta);
        aktivnostiMjesta.SequenceEqual(mjesto.aktivnostiMjesta);

    }
}
