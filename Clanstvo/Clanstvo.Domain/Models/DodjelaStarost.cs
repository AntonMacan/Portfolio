using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clanstvo.Domain.Models;
public class DodjelaStarost : ValueObject
{
    private DateTime _datum;
    private RangStarost _rang;

    public DodjelaStarost(DateTime datum, RangStarost rang)
    {
        _datum = datum;
        _rang = rang ?? throw new ArgumentNullException(nameof(rang));
    }

    public DateTime Datum { get => _datum; set => _datum = value; }
    public RangStarost RangStarost { get => _rang; set => _rang = value; }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
                obj is DodjelaStarost dodjela &&
               _datum == dodjela._datum &&
               _rang.Equals(dodjela._rang);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_datum, _rang);
    }
}