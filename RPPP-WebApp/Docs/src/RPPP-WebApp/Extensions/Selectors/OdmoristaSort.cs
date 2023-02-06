using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;
using System;
using System.Linq;


namespace RPPP_WebApp.Extensions.Selectors
{
    public static class OdmoristaSort
    {
        public static IQueryable<Odmoriste> ApplySort(this IQueryable<Odmoriste> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Odmoriste, object>> orderSelector = null;
            System.Linq.Expressions.Expression<Func<Odmoriste, object>> secondOrderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = o => o.Naziv;
                    break;
                case 2:
                    orderSelector = o => o.Dionica.UlaznaPostajaNavigation.Ime;
                    secondOrderSelector = o => o.Dionica.IzlaznaPostajaNavigation.Ime;
                    break;
                case 3:
                    orderSelector = o => o.Smjer;
                    break;
                case 4:
                    orderSelector = o => o.StacionazaKm;
                    secondOrderSelector = o => o.StacionazaM;
                    break;
                case 5:
                    orderSelector = o => o.GeografskaDuzina;
                    break;
                case 6:
                    orderSelector = o => o.GeografskaSirina;
                    break;
                case 7:
                    orderSelector = o => o.NadmorskaVisina;
                    break;

            }

            if (orderSelector != null)
            {
                if (secondOrderSelector != null)
                {
                    query = ascending ?
                        query.OrderBy(orderSelector).ThenBy(secondOrderSelector) :
                        query.OrderByDescending(orderSelector).ThenByDescending(secondOrderSelector);
                } else
                {
                    query = ascending ?
                        query.OrderBy(orderSelector) :
                        query.OrderByDescending(orderSelector);
                }            

            }

            return query;
        }
    }
}
