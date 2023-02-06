using RPPP_WebApp.Models;

namespace RPPP_WebApp.Extensions.Selectors
{
    public static class KoncesionarSort
    {
        public static IQueryable<Koncesionari> ApplySort(this IQueryable<Koncesionari> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Koncesionari, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = k => k.NazivKoncesionara;
                    break;
                case 2:
                    orderSelector = k => k.Url;
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