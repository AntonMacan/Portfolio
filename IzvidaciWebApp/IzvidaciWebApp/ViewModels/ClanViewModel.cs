using IzvidaciWebApp.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IzvidaciWebApp.ViewModels;
public class ClanViewModel
{
    public int Id { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public DateTime DatumRodenja { get; set; }

    public byte[] Slika { get; set; }
    public string Adresa { get; set; }
    public bool ImaMaramu { get; set; }
    public DateTime? DatumMarama { get; set; }
    public string MjestoMarama { get; set; }
}
