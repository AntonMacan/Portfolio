using RPPP_WebApp.Models;
//using RPPP_WebApp.ModelsPartials;
using System;
using System.Linq;

namespace RPPP_WebApp.Extensions.Selectors
{
    public static class NaplatnaPostajaSort
    {
        public static IQueryable<ViewNaplatnaPostajaENC> ApplySort(this IQueryable<ViewNaplatnaPostajaENC> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<ViewNaplatnaPostajaENC, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = np => np.NaplatnaId;
                    break;
                case 2:
                    orderSelector = np => np.Ime;
                    break;
                case 3:
                    orderSelector = np => np.GeoDuzina;
                    break;
                case 4:
                    orderSelector = np => np.GeoSirina;
                    break;
            }

            if(orderSelector != null)
            {
                query = ascending ?
                        query.OrderBy(orderSelector) :
                        query.OrderByDescending(orderSelector);
            }

            return query;
        }
    }
}
