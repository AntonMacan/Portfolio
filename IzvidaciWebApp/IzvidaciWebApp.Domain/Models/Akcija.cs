using BaseLibrary;
using System.Security.Cryptography;

namespace IzvidaciWebApp.Domain.Models;
public class Akcija
{
    private int _IdAkcije;
    private string _Naziv;
    private int _MjestoPbr;
    private int _Organizator;
    private int _KontaktOsoba;
    private string _Vrsta;
    private List<Aktivnost> _aktivnostiAkcije;

    public Akcija(int id, string naziv, int mjestoPbr, int organizator, int kontaktOsoba, string vrsta, IEnumerable<Aktivnost>? aktivnostiAkcije = null)
    {
        _IdAkcije = id;
        _Naziv = naziv;
        _MjestoPbr = mjestoPbr;
        _Organizator = organizator;
        _KontaktOsoba = kontaktOsoba;
        _Vrsta = vrsta;
        _aktivnostiAkcije = aktivnostiAkcije?.ToList() ?? new List<Aktivnost>();
    }

    public int IdAkcije { get => _IdAkcije; set => _IdAkcije = value; }
    public string Naziv { get => _Naziv; set => _Naziv = value; }

    public int MjestoPbr { get => _MjestoPbr; set => _MjestoPbr = value; }

    public int Organizator { get => _Organizator; set => _Organizator = value; }

    public int KontaktOsoba { get => _KontaktOsoba; set => _KontaktOsoba = value; }

    public string Vrsta { get => _Vrsta; set => _Vrsta = value; }

    public IReadOnlyList<Aktivnost> AktivnostiAkcije => _aktivnostiAkcije.ToList();

    public override bool Equals(object? obj)
    {
        return obj is not null &&
            obj is Akcija akcija &&
            IdAkcije.Equals(akcija.IdAkcije) &&
            Naziv.Equals(akcija.Naziv) &&
            MjestoPbr.Equals(akcija.MjestoPbr) &&
            Organizator.Equals(akcija.Organizator) &&
            KontaktOsoba.Equals(akcija.KontaktOsoba);
        AktivnostiAkcije.SequenceEqual(akcija.AktivnostiAkcije);

    }
}