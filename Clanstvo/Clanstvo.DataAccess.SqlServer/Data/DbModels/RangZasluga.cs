﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Clanstvo.DataAccess.SqlServer.Data.DbModels
{
    public partial class RangZasluga
    {
        public RangZasluga()
        {
            ClanRangZasluga = new HashSet<ClanRangZasluga>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Naziv { get; set; }

        [InverseProperty("RangZasluga")]
        public virtual ICollection<ClanRangZasluga> ClanRangZasluga { get; set; }
    }
}