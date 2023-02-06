using System.ComponentModel.DataAnnotations;
using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.Models;

public class SlikaDenorm
{
    public int KameraId { get; set; }
    public string Naziv { get; set; }
    public double? GeografskaSirina { get; set; }
    public double? GeografskaDuzina { get; set; }
    [Display(Name = "Datum slike")]
    [ExcelFormat("dd.mm.yyyy")] 
    public DateTime Datum { get; set; }
    public string Smjer { get; set; }
    public string Url { get; set; }
}