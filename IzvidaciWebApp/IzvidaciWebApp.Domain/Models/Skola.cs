using BaseLibrary;

namespace IzvidaciWebApp.Domain.Models;
public class Skola
{
    private int _idSkole;
    private string _NazivSkole;
    private int _MjestoPbr;
    private int _Organizator;
    private int _KontaktOsoba;
    private readonly List<Edukacija> _edukacijeUSkoli;

    public Skola(int id, string nazivSkole, int mjestoPbr, int organizator, int kontaktOsoba, IEnumerable<Edukacija>? edukacijaUSkoli = null)
    {
        _idSkole = id;
        _NazivSkole = nazivSkole;
        _MjestoPbr = mjestoPbr;
        _Organizator = organizator;
        _KontaktOsoba = kontaktOsoba;
        _edukacijeUSkoli = edukacijaUSkoli?.ToList() ?? new List<Edukacija>();
    }
    public int IdSkole { get => _idSkole; set => _idSkole = value; }
    public string NazivSkole { get => _NazivSkole; set => _NazivSkole = value; }
    public int Organizator { get => _Organizator; set => _Organizator = value; }
    public int MjestoPbr { get => _MjestoPbr; set => _MjestoPbr = value; }
    public int KontaktOsoba { get => _KontaktOsoba; set => _KontaktOsoba = value; }
    public IReadOnlyList<Edukacija> EdukacijeUSkoli => _edukacijeUSkoli.ToList();
}

