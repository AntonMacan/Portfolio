﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RPPP_WebApp.Models
{
    public partial class Koncesionari
    {
        public Koncesionari()
        {
            Autoceste = new HashSet<Autoceste>();
        }

        public string NazivKoncesionara { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Autoceste> Autoceste { get; set; }
    }
}