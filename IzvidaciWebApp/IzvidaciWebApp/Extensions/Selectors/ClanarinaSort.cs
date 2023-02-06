using System.Linq.Expressions;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Extensions.Selectors;

public static class ClanarinaSort
{
    public static IQueryable<Clanarina> ApplySort(this IQueryable<Clanarina> query, int sort, bool ascending)
    {
        System.Linq.Expressions.Expression<Func<Clanarina, object>> orderSelector = null;
        switch (sort)
        {
            case 1:
                orderSelector = a => a.Id;
                break;
            case 2:
                orderSelector = a => a.Godina;
                break;
            case 3:
                orderSelector = a => a.Iznos;
                break;
            case 4:
                orderSelector = a => a.Placenost;
                break;
            case 5:
                orderSelector = a => a.Datum;
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