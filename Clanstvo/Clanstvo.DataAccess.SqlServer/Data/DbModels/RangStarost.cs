﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Clanstvo.DataAccess.SqlServer.Data.DbModels
{
    public partial class RangStarost
    {
        public RangStarost()
        {
            ClanRangStarost = new HashSet<ClanRangStarost>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Naziv { get; set; }

        [InverseProperty("RangStarost")]
        public virtual ICollection<ClanRangStarost> ClanRangStarost { get; set; }
    }
}