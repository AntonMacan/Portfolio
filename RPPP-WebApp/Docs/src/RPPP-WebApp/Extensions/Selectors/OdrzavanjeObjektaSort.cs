using RPPP_WebApp.Models;

namespace RPPP_WebApp.Extensions.Selectors;
/// <summary>
/// Statična klasa koja sortira održavanja objekta
/// </summary>
public static class OdrzavanjeObjektaSort
{
    /// <summary>
    /// Statična metoda koja sortira održavanja objekta
    /// </summary>
    /// <param name="query">Lista održavanja objekta</param>
    /// <param name="sort">Index stupca po kojem se sortira</param>
    /// <param name="ascending">Smjer po kojem se sortira</param>
    /// <returns>Listu sortiranih održavanja objekta</returns>
    public static IQueryable<OdrzavanjeObjekta> ApplySort2(this IQueryable<OdrzavanjeObjekta> query, int sort,
        bool ascending)
    {
        System.Linq.Expressions.Expression<Func<OdrzavanjeObjekta, object>> orderSelector = null;
        switch (sort)
        {
            case 1:
                orderSelector = k => k.Id;
                break;
            case 2:
                orderSelector = k => k.Datum;
                break;
            case 3:
                orderSelector = k => k.Odrzavatelj;
                break;
            case 4:
                orderSelector = k => k.TipId;
                break;
            case 5:
                orderSelector = k => k.Ishod;
                break;
            case 6:
                orderSelector = k => k.Opis;
                break;
        }
        if (orderSelector != null)
        {
            query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
        }

        return query;
    }
}