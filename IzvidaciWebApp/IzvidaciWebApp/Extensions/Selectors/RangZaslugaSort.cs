using System.Linq.Expressions;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Extensions.Selectors;

public static class RangZaslugaSorter
{
    public static IQueryable<RangZasluga> ApplySort(this IQueryable<RangZasluga> query, int sort, bool ascending)
    {
        Expression<Func<RangZasluga, object>> orderSelector = sort switch
        {
            1 => r => r.Id,
            2 => r => r.Naziv,
            _ => null
        };
      
        if (orderSelector != null)
        {
            query = ascending ?
                query.OrderBy(orderSelector) :
                query.OrderByDescending(orderSelector);
        }

        return query;
    }
}