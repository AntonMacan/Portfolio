using BaseLibrary;
using System;
using System.Data;

namespace IzvidaciWebApp.Domain.Models;
public class Edukacija
{
    private int _id;
    private string _NazivEdukacija;
    private int _MjestoPbr;
    private string _OpisEdukacije;
    private int _SkolaId;
    private readonly List<PredavacNaEdukaciji> _predavaciNaEdukaciji;
    private readonly List<PolaznikNaEdukaciji> _polazniciNaEdukaciji;
    private readonly List<PrijavljeniClanNaEdukaciji> _prijavljeniNaEdukaciju;

    public Edukacija(int id, string nazivEdukacije, int mjestoPbr, string opisEdukacije, int skolaId, IEnumerable<PredavacNaEdukaciji>? predavaciNaEdukaciji = null, IEnumerable<PolaznikNaEdukaciji>? polazniciEdukacije = null, IEnumerable<PrijavljeniClanNaEdukaciji>? prijavljeniNaEdukaciju = null)
    {
        _id = id;
        _NazivEdukacija = nazivEdukacije;
        _MjestoPbr = mjestoPbr;
        _OpisEdukacije = opisEdukacije;
        _SkolaId = skolaId;
        _predavaciNaEdukaciji = predavaciNaEdukaciji?.ToList() ?? new List<PredavacNaEdukaciji>();
        _polazniciNaEdukaciji = polazniciEdukacije?.ToList() ?? new List<PolaznikNaEdukaciji>();
        _prijavljeniNaEdukaciju = prijavljeniNaEdukaciju?.ToList() ?? new List<PrijavljeniClanNaEdukaciji>();
    }
    public int Id { get => _id; set => _id = value; }
    public string NazivEdukacije { get => _NazivEdukacija; set => _NazivEdukacija = value; }
    public int MjestoPbr { get => _MjestoPbr; set => _MjestoPbr = value; }
    public string OpisEdukacije { get => _OpisEdukacije; set => _OpisEdukacije = value; }
    public int SkolaId { get => _SkolaId; set => _SkolaId = value; }
    public IReadOnlyList<PredavacNaEdukaciji> PredavaciNaEdukaciji => _predavaciNaEdukaciji.ToList();
    public IReadOnlyList<PolaznikNaEdukaciji> PolazniciEdukacije => _polazniciNaEdukaciji.ToList();
    public IReadOnlyList<PrijavljeniClanNaEdukaciji> PrijavljeniNaEdukaciji => _prijavljeniNaEdukaciju.ToList();

}

