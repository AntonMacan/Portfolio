using System.ComponentModel.DataAnnotations;

namespace IzvidaciWebApp.Domain.Models;
public class RangZasluga 
{
    private readonly int _id;
    private readonly string _naziv;
    public int Id { get; set; }
    public string Naziv { get; set; }
}