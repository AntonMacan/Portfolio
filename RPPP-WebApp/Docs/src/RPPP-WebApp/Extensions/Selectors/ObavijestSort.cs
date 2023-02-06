using RPPP_WebApp.Models;

namespace RPPP_WebApp.Extensions.Selectors
{
    public static class ObavijestSort
    {
        public static IQueryable<Obavijesti> ApplySort(this IQueryable<Obavijesti> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Obavijesti, object>> orderSelector = null;
            System.Linq.Expressions.Expression<Func<Obavijesti, object>> secondOrderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = o => o.Naslov;
                    break;
                case 2:
                    orderSelector = o => o.IdDioniceNavigation.UlaznaPostajaNavigation.Ime;
                    secondOrderSelector = o => o.IdDioniceNavigation.IzlaznaPostajaNavigation.Ime;
                    break;
                case 3:
                    orderSelector = o => o.VrijemeObjave;
                    break;
            }

            if (orderSelector != null)
            {
                if (secondOrderSelector != null)
                {
                    query = ascending ?
                        query.OrderBy(orderSelector).ThenBy(secondOrderSelector) :
                        query.OrderByDescending(orderSelector).ThenByDescending(secondOrderSelector);
                }
                else
                {
                    query = ascending ?
                        query.OrderBy(orderSelector) :
                        query.OrderByDescending(orderSelector);
                }

            }

            return query;
        }
    }
}