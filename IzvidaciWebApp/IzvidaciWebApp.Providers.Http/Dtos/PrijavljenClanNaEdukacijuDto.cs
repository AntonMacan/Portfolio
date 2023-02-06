

using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers.Http.Dtos
{
    public class PrijavljenClanNaEdukacijuDto
    {
        public int IdPolaznik { get; set; }
        public DateTime datum { get; set; }

    }

    public static partial class DtoMapping
    {
        public static PrijavljenClanNaEdukacijuDto ToDto(this PrijavljeniClanNaEdukaciji prijavljeni)
        {
            return new PrijavljenClanNaEdukacijuDto()
            {
                IdPolaznik = prijavljeni.idPolaznik,
                datum = prijavljeni.datumPrijave
            };
        }

        public static PrijavljeniClanNaEdukaciji ToDomain(this PrijavljenClanNaEdukacijuDto prijavljeni)
        {
            return new PrijavljeniClanNaEdukaciji(
                prijavljeni.IdPolaznik,
                prijavljeni.datum
                );
        }
    }
}
