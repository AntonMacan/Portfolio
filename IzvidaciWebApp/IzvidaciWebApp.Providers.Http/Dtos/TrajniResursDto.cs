using System.ComponentModel.DataAnnotations;
using DomainModels = IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos;

public class TrajniResursDto : ResursDto
{

    [Required(ErrorMessage = "InventarniBroj can't be null")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "InventarniBroj must be 6 characters")]
    public String InventarniBroj { get; set; } = String.Empty;
    
    public bool JeDostupno { get; set; }
    
}

public static partial class DtoMapping
{
    public static TrajniResursDto ToDto(this DomainModels.TrajniResurs resurs)
        => new TrajniResursDto()
        {
            Id = resurs.Id,
            IdUdruge = resurs.IdUdruge,
            IdProstor = resurs.IdProstor,
            DatumNabave = resurs.DatumNabave,
            InventarniBroj = resurs.InventarniBroj,
            JeDostupno = resurs.JeDostupno,
            Napomena = resurs.Napomena,
            Naziv = resurs.Naziv
        };

    public static DomainModels.TrajniResurs ToDomain(this TrajniResursDto resurs)
        => new DomainModels.TrajniResurs(
            resurs.Id,
            resurs.Naziv,
            resurs.Napomena,
            resurs.DatumNabave,
            resurs.IdUdruge,
            resurs.Udruga,
            resurs.IdProstor,
            resurs.Prostor,
            resurs.InventarniBroj,
            resurs.JeDostupno
        );
}