// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RPPP_WebApp.Models
{
    public partial class Cjenik
    {
        public int UlazId { get; set; }
        public int IzlazId { get; set; }
        public int CijenaIa { get; set; }
        public int CijenaI { get; set; }
        public int CijenaIi { get; set; }
        public int CijenaIii { get; set; }
        public int CijenaIv { get; set; }

        public virtual NaplatnaPostaja Izlaz { get; set; }
        public virtual NaplatnaPostaja Ulaz { get; set; }
    }
}