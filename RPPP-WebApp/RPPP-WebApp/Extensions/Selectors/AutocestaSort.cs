using RPPP_WebApp.Models;


namespace RPPP_WebApp.Extensions.Selectors
{
    public static class AutocestaSort
    {
        public static IQueryable<Autoceste> ApplySort(this IQueryable<Autoceste> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Autoceste, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = a => a.Oznaka;
                    break;
                case 2:
                    orderSelector = a => a.Koncesionar;
                    break;
                case 3:
                    orderSelector = a => a.Pocetak;
                    break;
                case 4:
                    orderSelector = a => a.Kraj;
                    break;
                case 5:
                    orderSelector = a => a.DuljinaKm;
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