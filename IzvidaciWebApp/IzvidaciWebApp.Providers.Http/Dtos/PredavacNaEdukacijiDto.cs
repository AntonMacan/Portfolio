
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos
{
    public class PredavacNaEdukacijiDto
    {
        public int IdPredavac { get; set; }
        public int IdClan { get; set; }

    }

    public static partial class DtoMapping 
    {
        public static PredavacNaEdukacijiDto ToDto(this PredavacNaEdukaciji predavacNaEdukaciji)
        {
            return new PredavacNaEdukacijiDto()
            {
                IdPredavac = predavacNaEdukaciji.idPredavac,
                IdClan = predavacNaEdukaciji.idClan
            };
        }

        public static PredavacNaEdukaciji ToDomain(this PredavacNaEdukacijiDto predavacNaEdukaciji) {
            return new PredavacNaEdukaciji(
                predavacNaEdukaciji.IdPredavac,
                predavacNaEdukaciji.IdClan
                );
        }
    }
}
