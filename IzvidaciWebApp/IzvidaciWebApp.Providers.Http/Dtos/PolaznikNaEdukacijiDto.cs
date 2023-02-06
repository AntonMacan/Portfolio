using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos;

public class PolaznikNaEdukacijiDto
{
    public int IdPolaznik { get; set; }

}

public static partial class DtoMapping
{
    public static PolaznikNaEdukacijiDto ToDto(this PolaznikNaEdukaciji polaznikSkole)
    {
        return new PolaznikNaEdukacijiDto()
        {
            IdPolaznik = polaznikSkole.idPolaznik
        };
    }

    public static PolaznikNaEdukaciji ToDomain(this PolaznikNaEdukacijiDto polaznikSkole)
    {
        return new PolaznikNaEdukaciji(
            polaznikSkole.IdPolaznik
            );
    }
}
