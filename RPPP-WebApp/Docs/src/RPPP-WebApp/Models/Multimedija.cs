﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RPPP_WebApp.Models
{
    public partial class Multimedija
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Url { get; set; }
        public int OdmoristeId { get; set; }

        public virtual Odmoriste Odmoriste { get; set; }
    }
}