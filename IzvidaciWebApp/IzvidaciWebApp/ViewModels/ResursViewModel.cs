using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.CompilerServices;

namespace IzvidaciWebApp.ViewModels;

public class ResursViewModel
{
    private DateOnly _datumNabave;
    
    public int IdResurs { get; set; }
    
    [Required]
    public int IdUdruga { get; set; }
    
    public string? Udruga { get; set; }
    
    [Required(ErrorMessage = "Potrebno je odabrati prostor", AllowEmptyStrings = false)]
    public int IdProstor { get; set; } 
    
    public string? Prostor { get; set; }
    
    [Required(ErrorMessage = "Potrebno je unijeti naziv")]
    [StringLength(5, ErrorMessage = "Naziv ne moze biti dulji od 50 slova")]
    [MinLength(1)]
    public string Naziv { get; set; } 
    
    
    public DateTime DatumNabaveEdit { get; set; }
    
    public DateOnly DatumNabave { get => _datumNabave;
        set => _datumNabave = value;
    }
    
    [StringLength(50, ErrorMessage = "Naziv ne moze biti dulji od 50 slova")]
    public string? Napomena { get; set; }
}