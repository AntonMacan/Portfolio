using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;

namespace RPPP_WebApp.Extensions.Selectors
{
    /// <summary>
    /// Statična klasa koja sortira kamere
    /// </summary>

    public static class KamereSort
    {
        /// <summary>
        /// Statična metoda koja sortira kamere
        /// </summary>
        /// <param name="query">Lista kamera</param>
        /// <param name="sort">Index stupca po kojem se sortira</param>
        /// <param name="ascending">Smjer po kojem se sortira</param>
        /// <returns>Listu sortiranih kamera</returns>
        public static IQueryable<Kamere> ApplySort(this IQueryable<Kamere> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Kamere, object>> orderSelector = null;

            switch (sort)
            {
                case 1:
                    orderSelector = k => k.Id;
                    break;
                case 2:
                    orderSelector = k => k.Naziv;
                    break;
                case 3:
                    orderSelector = k => k.Dionica.Id;
                    break;
                case 4:
                    orderSelector = k => k.GeografskaSirina;
                    break;
                case 5:
                    orderSelector = k => k.GeografskaDuzina;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            return query;
        }
    }
}