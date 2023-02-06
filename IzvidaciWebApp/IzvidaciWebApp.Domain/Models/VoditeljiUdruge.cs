using BaseLibrary;
using System;
using System.Data;

namespace IzvidaciWebApp.Domain.Models
{
    public class VoditeljiUdruge
    {

        private int _IdUdruge;
        private int _IdClan;
        private string _Pozicija;
        private DateTime? _NaPozicijiDo;
       

        public VoditeljiUdruge(int idUdruge, int idClan, string pozicija, DateTime? naPozicijiDo)
        {
            _IdUdruge = idUdruge;
            _IdClan = idClan;
            _Pozicija = pozicija;
            _NaPozicijiDo = naPozicijiDo;
        }

        public int IdUdruge { get => _IdUdruge; set => _IdUdruge = value; }
        public int IdClan { get => _IdClan; set => _IdClan = value; }
        public string Pozicija { get => _Pozicija; set => _Pozicija = value; }
        public DateTime? NaPozicijiDo { get => _NaPozicijiDo; set => _NaPozicijiDo = value; }
  




    }
}

