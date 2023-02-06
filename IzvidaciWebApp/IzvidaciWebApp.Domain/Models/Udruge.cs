using BaseLibrary;
using System;
using System.Data;

namespace IzvidaciWebApp.Domain.Models
{
    public class Udruge
    {

        private int _IdUdruge;
        private string _OIB;
        private string _Naziv;
        private string _Sjediste;
        private string _BrMob;
        private string _Mail;

        public Udruge(int id, string oib, string naziv, string sjediste, string brmob, string mail)
        {
            _IdUdruge = id;
            _OIB = oib; 
            _Naziv = naziv;
            _Sjediste = sjediste;
            _BrMob = brmob;
            _Mail = mail;
        }

        public int IdUdruge { get => _IdUdruge; set => _IdUdruge = value; }
        public string OIB { get => _OIB; set => _OIB = value; }
        public string Naziv { get => _Naziv; set => _Naziv = value; }
        public string Sjediste { get => _Sjediste; set => _Sjediste = value; }

        public string BrMob { get => _BrMob; set => _BrMob = value; }
        public string Mail { get => _Mail; set => _Mail = value; }

        

        


    }
}

