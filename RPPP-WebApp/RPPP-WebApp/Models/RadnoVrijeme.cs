﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RPPP_WebApp.Models
{
    public partial class RadnoVrijeme
    {
        public int SadrzajId { get; set; }
        public int Dan { get; set; }
        public TimeSpan VrijemePocetka { get; set; }
        public TimeSpan VrijemeZavrsetka { get; set; }

        public virtual Sadrzaj Sadrzaj { get; set; }
    }
}