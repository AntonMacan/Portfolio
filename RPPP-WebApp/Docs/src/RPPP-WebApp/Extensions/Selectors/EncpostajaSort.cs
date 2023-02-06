using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;
namespace RPPP_WebApp.Extensions.Selectors
{
    public static class EncpostajaSort
    {
        public static IQueryable<Encpostaja> ApplySort(this IQueryable<Encpostaja> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Encpostaja, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = enc => enc.Encid;
                    break;
                case 2:
                    orderSelector = enc => enc.Ime;
                    break;
                case 3:
                    orderSelector = enc => enc.VrijemeOtvaranja;
                    break;
                case 4:
                    orderSelector = enc => enc.VrijemeZatvaranja;
                    break;
                case 5:
                    orderSelector = enc => enc.KontaktBroj;
                    break;
                case 6:
                    orderSelector = enc => enc.Naplatna.Ime;
                    break;
                case 7:
                    orderSelector = enc => enc.NaplatnaStaza;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ?
                        query.OrderBy(orderSelector) :
                        query.OrderByDescending(orderSelector);
            }

            return query;
        }
    }
}
