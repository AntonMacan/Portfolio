using IzvidaciWebApp.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IzvidaciWebApp.ViewModels;
public class ClanarinaViewModel
{
    public int Id { get; set; }
    public bool Placenost { get; set; }
    public decimal Iznos { get; set; }
    public int Godina { get; set; }

    public int ClanId { get; set; }
    public string ClanIme { get; set; }
    public string ClanPrezime { get; set; }
    public DateTime? Datum { get; set; }
    //public virtual Clan Clan { get; set; }
}

