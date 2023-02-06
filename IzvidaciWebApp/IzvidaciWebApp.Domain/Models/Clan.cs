namespace IzvidaciWebApp.Domain.Models;

public class Clan
{
    private readonly int _id;
    private readonly string _ime;
    private readonly string _prezime;
    private readonly DateTime _datumRodenja;
    private readonly byte[]? _slika;
    private readonly string _adresa;
    private readonly bool _imaMaramu;
    private readonly DateTime? _datumMarama;
    private readonly string? _mjestoMarama;
    private readonly int _udrugaId;

    /*
    private readonly List<DodjelaZasluga> _dodjeleZasluga;
    private readonly List<DodjelaStarost> _dodjeleStarost;
    private readonly List<Clanarina> _clanarine;
    private int id;
    private string ime;
    private string prezime;
    private DateTime datumRodenja;
    private byte[]? slika;
    private string adresa;
    private bool imaMaramu;
    private DateTime? datumMarama;
    private string? mjestoMarama;
    */

    public int Id => _id;
    public string Ime => _ime;
    public string Prezime => _prezime;
    public DateTime DatumRodenja => _datumRodenja;
    public byte[]? Slika => _slika;
    public string Adresa => _adresa;
    public bool ImaMaramu => _imaMaramu;
    public DateTime? DatumMarama => _datumMarama;
    public string? MjestoMarama => _mjestoMarama;
    //public int udrugaId => _udrugaId;

    //public IReadOnlyList<DodjelaStarost> DodjeleStarost => _dodjeleStarost.ToList();
    //public IReadOnlyList<DodjelaZasluga> DodjeleZasluga => _dodjeleZasluga.ToList();
    //public IReadOnlyList<Clanarina> Clanarina => _clanarine.ToList();

    /*
    public Clan(int id, string ime, string prezime, DateTime datumRodenja,
                      byte[]? slika, string adresa, bool imaMaramu, DateTime? datumMarama, string? mjestoMarama,
                      int udrugaId,IEnumerable<DodjelaStarost>? rangoviStarost = null,
                      IEnumerable<DodjelaZasluga>? rangoviZasluga = null,
                      IEnumerable<Clanarina>? clanarine = null)
    {
        _id = id;
        _ime = ime;
        _prezime = prezime;
        _datumRodenja = datumRodenja;
        _slika = slika;
        _adresa = adresa;
        _imaMaramu = imaMaramu;
        _datumMarama = datumMarama;
        _mjestoMarama = mjestoMarama;
        _dodjeleStarost = rangoviStarost?.ToList() ?? new List<DodjelaStarost>();
        _dodjeleZasluga = rangoviZasluga?.ToList() ?? new List<DodjelaZasluga>();
        _clanarine = clanarine?.ToList() ?? new List<Clanarina>();
        _udrugaId = udrugaId;
    }
    */

    public Clan(int id, string ime, string prezime, DateTime datumRodenja, byte[]? slika, string adresa, bool imaMaramu, DateTime? datumMarama, string? mjestoMarama)
    {
        _id = id;
        _ime = ime;
        _prezime = prezime;
        _datumRodenja = datumRodenja;
        _slika = slika;
        _adresa = adresa;
        _imaMaramu = imaMaramu;
        _datumMarama = datumMarama;
        _mjestoMarama = mjestoMarama;
    }
}
