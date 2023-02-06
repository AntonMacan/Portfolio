using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Domain.Models
{
    public class MaterijalnaPotreba
    {
        private int _IdMaterijalnaPotreba;
        private string _Naziv;
        private int _Organizator;
        private int _Davatelj;
        private bool _Zadovoljeno;

        public MaterijalnaPotreba(int id, string naziv, int organizator, int davatelj, bool zadovoljeno)
        {
            _IdMaterijalnaPotreba = id;
            _Naziv = naziv;
            _Organizator = organizator;
            _Davatelj = davatelj;
            _Zadovoljeno = zadovoljeno;
        }

        public int IdMaterijalnaPotreba { get => _IdMaterijalnaPotreba; set => _IdMaterijalnaPotreba = value; }
        public string Naziv { get => _Naziv; set => _Naziv = value; }

        public int Organizator { get => _Organizator; set => _Organizator = value; }
        public int Davatelj { get => _Davatelj; set => _Davatelj = value; }

        public bool Zadovoljeno { get => _Zadovoljeno; set => _Zadovoljeno = value; }

        public override bool Equals(object? obj)
        {
            return obj is not null &&
                obj is MaterijalnaPotreba potreba &&
                IdMaterijalnaPotreba.Equals(potreba.IdMaterijalnaPotreba) &&
                Naziv.Equals(potreba.Naziv);
            Organizator.Equals(potreba.Organizator);
            Davatelj.Equals(potreba.Davatelj);
            Zadovoljeno.Equals(potreba.Zadovoljeno);

        }
    }
}
