using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clanstvo.Domain.Models;
public class DodjelaZasluga : ValueObject
{
    private DateTime _datum;
    private RangZasluga _rang;

    public DodjelaZasluga(DateTime datum, RangZasluga rang)
    {
        _datum = datum;
        _rang = rang ?? throw new ArgumentNullException(nameof(rang));
    }

    public DateTime Datum { get => _datum; set => _datum = value; }
    public RangZasluga RangZasluga { get => _rang; set => _rang = value; }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is DodjelaZasluga dodjela &&
               _datum == dodjela._datum &&
               _rang.Equals(dodjela._rang);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_datum, _rang);
    }
}