﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.Models
{
    public partial class Odmoriste
    {
        public Odmoriste()
        {
            Multimedija = new HashSet<Multimedija>();
            Sadrzaj = new HashSet<Sadrzaj>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public string Smjer { get; set; }
        public int? NadmorskaVisina { get; set; }
        public int? StacionazaKm { get; set; }
        public int? StacionazaM { get; set; }
        public double? GeografskaSirina { get; set; }
        public double? GeografskaDuzina { get; set; }
        public int DionicaId { get; set; }

        public string NazivDionice 
        {
            get
            {
                try
                {
                    return Converters.GetDionicaName(this.Dionica);
                }
                catch (NullReferenceException)
                {
                    return "";
                }
            }
        }

        public virtual Dionice Dionica { get; set; }
        public virtual ICollection<Multimedija> Multimedija { get; set; }
        public virtual ICollection<Sadrzaj> Sadrzaj { get; set; }
    }
}