using RPPP_WebApp.Models;

namespace RPPP_WebApp.Extensions.Selectors;
/// <summary>
/// Statična klasa koja sortira objekte
/// </summary>
public static class ObjektSort
{
    /// <summary>
    /// Statična metoda koja sortira objekte
    /// </summary>
    /// <param name="query">Lista objekata</param>
    /// <param name="sort">Index stupca po kojem se sortira</param>
    /// <param name="ascending">Smjer po kojem se sortira</param>
    /// <returns>Listu sortiranih objekata</returns>
    public static IQueryable<Objekt> ApplySort(this IQueryable<Objekt> query, int sort, bool ascending)
    {
        System.Linq.Expressions.Expression<Func<Objekt, object>> orderSelector = null;

        switch (sort)
        {
            case 2:
                orderSelector = k => k.Id;
                break;
            case 3:
                orderSelector = k => k.Naziv;
                break;
            case 4:
                orderSelector = k => k.Dionica.Id;
                break;
            case 5:
                orderSelector = k => k.TipObjekta;
                break;
            case 6:
                orderSelector = k => k.NadmorskaVisina;
                break;
            case 7:
                orderSelector = k => k.Stacionaza;
                break;
            case 8:
                orderSelector = k => k.DimenzijeM;
                break;
            case 9:
                orderSelector = k => k.GeografskaSirina;
                break;
            case 10:
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
