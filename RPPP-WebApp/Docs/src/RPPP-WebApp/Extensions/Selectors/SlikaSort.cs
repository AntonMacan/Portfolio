using Microsoft.EntityFrameworkCore.Query;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.Extensions.Selectors;
/// <summary>
/// Statična klasa koja sortira slike
/// </summary>
public static class SlikaSort
{
    /// <summary>
    /// Statična metoda koja sortira slike
    /// </summary>
    /// <param name="query">Lista slika</param>
    /// <param name="sort">Index stupca po kojem se sortira</param>
    /// <param name="ascending">Smjer po kojem se sortira</param>
    /// <returns>Listu sortiranih slika</returns>
    public static IQueryable<Slike> ApplySort3(this IQueryable<Slike> query, int sort, bool ascending)
    {
        System.Linq.Expressions.Expression<Func<Slike, object>> orderSelector = null;
        
        switch (sort)
        {
            case 1:
                orderSelector = k => k.Kamera.Naziv;
                break;
            case 2:
                orderSelector = k => k.Datum;
                break;
            case 3:
                orderSelector = k => k.Smjer;
                break;
            case 4:
                orderSelector = k => k.Url;
                break;
        }
        if (orderSelector != null)
        {
            query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
        }

        return query;
    }
}