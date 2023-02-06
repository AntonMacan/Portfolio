namespace IzvidaciWebApp.Providers.Http.Dtos;

public class RangStarostDTO
{
    public int id { get; set; }
    public string naziv { get; set; }
    
}

public static partial class DtoMapping
{
    public static RangStarostDTO ToDto(this Domain.Models.RangStarost clan)
        => new RangStarostDTO
        {
            id = clan.Id,
            naziv = clan.Naziv
        };

    public static Domain.Models.RangStarost ToDomain(this RangStarostDTO clan
    )
        => new Domain.Models.RangStarost
        {
            Id = clan.id, 
            Naziv = clan.naziv 
        };
}
