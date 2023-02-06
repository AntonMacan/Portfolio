
namespace Clanstvo.Providers.Http.DTOs
{


    public class SkolaSudionik
    {
        public int IdSkole { get; set; }

        public string NazivSkole { get; set; } = String.Empty;




        public static partial class DtoMapping
        {
            public static SkolaSudionik ToDto(Domain.Models.Skola skola)
            {
                return new SkolaSudionik()
                {
                    IdSkole = skola.Id,
                    NazivSkole = skola.NazivSkole
                };
            }


            public static Domain.Models.Skola ToDomain(SkolaSudionik skola)
            {
                return new Domain.Models.Skola(skola.IdSkole, skola.NazivSkole);
            }

        }
    }
}
