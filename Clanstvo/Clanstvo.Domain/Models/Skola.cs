using BaseLibrary;
using Clanstvo.Commons;
using Clanstvo.Domain;

namespace Clanstvo.Domain.Models;
public class Skola : AggregateRoot<int>
{
    private string _NazivSkole;
    

    public Skola(int id, string nazivSkole) : base(id)
    {
        if (string.IsNullOrEmpty(nazivSkole))
        {
            throw new ArgumentException($"'{nameof(nazivSkole)}' cannot be null or empty.", nameof(nazivSkole));
        }
        _NazivSkole = nazivSkole;
       
    }

    public string NazivSkole { get => _NazivSkole; set => _NazivSkole = value; }
    

    public override bool Equals(object? obj)
    {
        return obj is not null &&
               obj is Skola skola &&
               Id.Equals(skola.Id) &&
               NazivSkole.Equals(skola.NazivSkole);

    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, NazivSkole);
    }

    /*
    public override Result IsValid()
        => Validation.Validate(
            (() => _NazivSkole.Length <= 50, "Naziv skole lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_NazivSkole.Trim()), "Naziv skole name can't be null, empty, or whitespace")
            );

    */


}

