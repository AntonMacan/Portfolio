using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clanstvo.Domain.Models;
public class RangStarost : Entity<int>
{
    private string _naziv;

    public RangStarost(int id, string naziv) : base(id)
    {
        if (string.IsNullOrEmpty(naziv))
        {
            throw new ArgumentException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
        }

        _naziv = naziv;
    }

    public string Naziv { get => _naziv; set => _naziv = value; }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
               obj is RangStarost rang &&
               Id.Equals(rang.Id) &&
               Naziv.Equals(rang.Naziv);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Naziv);
    }
}


 /*  public override Result IsValid()
        => Validation.Validate(
            (() => _naziv.Length <= 50, "RangStarost naziv lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_name.Trim()), "RangStarost naziv can't be null, empty, or whitespace")
            );
*/