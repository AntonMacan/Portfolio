using System.Linq.Expressions;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Extensions.Selectors;

public static class RangStarostSort
{
    public static IQueryable<RangStarost> ApplySort(this IQueryable<RangStarost> query, int sort, bool ascending)
    {
        Expression<Func<RangStarost, object>> orderSelector = sort switch
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