// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RPPP_WebApp.Models
{
    public partial class Obavijesti
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public int? IdDionice { get; set; }
        public DateTime VrijemeObjave { get; set; }
        public string Naslov { get; set; }

        public virtual Dionice IdDioniceNavigation { get; set; }
    }
}