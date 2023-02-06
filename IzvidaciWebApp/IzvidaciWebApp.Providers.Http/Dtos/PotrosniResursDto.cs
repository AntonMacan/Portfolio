using System.ComponentModel.DataAnnotations;
using DomainModels = IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos;

public class PotrosniResursDto : ResursDto
{

    [Required(ErrorMessage = "RokTrajanja can't be null")]
    [DataType(DataType.DateTime)]
    public DateTime RokTrajanja { get; set; }
    
}

public static partial class DtoMapping
{
    public static PotrosniResursDto ToDto(this DomainModels.PotrosniResurs resurs)
        => new PotrosniResursDto()
        {
            Id = resurs.Id,
            IdUdruge = resurs.IdUdruge,
            IdProstor = resurs.IdProstor,
            DatumNabave = resurs.DatumNabave,
            RokTrajanja= resurs.RokTrajanja,
            Napomena = resurs.Napomena,
            Naziv = resurs.Naziv
        };

    public static DomainModels.PotrosniResurs ToDomain(this PotrosniResursDto resurs)
        => new DomainModels.PotrosniResurs(
            resurs.Id,
            resurs.Naziv,
            resurs.Napomena,
            resurs.DatumNabave,
            resurs.IdUdruge,
            resurs.Udruga,
            resurs.IdProstor,
            resurs.Prostor,
            resurs.RokTrajanja
        );
}