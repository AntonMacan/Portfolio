using RPPP_WebApp.Models;

namespace RPPP_WebApp.Extensions.Selectors
{
    public static class DionicaSort
    {
        public static IQueryable<Dionice> ApplySort(this IQueryable<Dionice> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Dionice, object>> orderSelector = null;
            System.Linq.Expressions.Expression<Func<Dionice, object>> secondOrderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.UlaznaPostajaNavigation.Ime;
                    secondOrderSelector = d => d.IzlaznaPostajaNavigation.Ime;
                    break;
                case 2:
                    orderSelector = d => d.OznakaAutoceste;
                    break;
                case 3:
                    orderSelector = d => d.DuljinaKm;
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