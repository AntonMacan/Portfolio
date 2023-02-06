using BaseLibrary;
using Clanstvo.Domain;

namespace Clanstvo.Domain.Models;

    public class Akcija : AggregateRoot<int>
    {
        private string _Naziv;
        

        public Akcija(int id, string naziv) : base(id)
        {
            if (string.IsNullOrEmpty(naziv))
            {
                throw new ArgumentNullException($"'{nameof(naziv)}' cannot be null or empty.", nameof(naziv));
            }
            _Naziv = naziv;
            
        }

        public string Naziv { get => _Naziv; set => _Naziv = value; }

        

        public override bool Equals(object? obj)
        {
            return obj is not null &&
                obj is Akcija akcija &&
                Id.Equals(akcija.Id) &&
                Naziv.Equals(akcija.Naziv);

        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Naziv);
        }

        /*
        public override Result IsValid()
        => Validation.Validate(
            (() => _Naziv.Length <= 50, "Naziv akcije lenght must be less than 50 characters"),
            (() => !string.IsNullOrEmpty(_Naziv.Trim()), "Naziv akcije name can't be null, empty, or whitespace")
            );
        */
    }


