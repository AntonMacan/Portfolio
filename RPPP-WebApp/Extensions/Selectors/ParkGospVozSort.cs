using NuGet.ProjectModel;
using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;
using System;
using System.Linq;

namespace RPPP_WebApp.Extensions.Selectors
{
    public static class ParkGospVozSort
    {
        public static IQueryable<ParkGospVoz> ApplySort(this IQueryable<ParkGospVoz> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<ParkGospVoz, object>> orderSelector = null;
            System.Linq.Expressions.Expression<Func<ParkGospVoz, object>> secondOrderSelector = null;

            switch (sort)
            {
                case 1:
                    orderSelector = p => p.ParkingId;
                    break;
                case 2:
                    orderSelector = p => p.Stacionaža;
                    break;
                case 3:
                    orderSelector = p => p.Naziv;
                    break;

                case 4:
                    orderSelector = p => p.Dionica.UlaznaPostajaNavigation.Ime;
                    secondOrderSelector = d => d.Dionica.IzlaznaPostajaNavigation.Ime;                                     
                    break;
                    

                case 5:
                    orderSelector = p => p.GeoDuzinaUlaz;
                    break;
                    
                case 6:
                    orderSelector = p => p.GeoSirinaUlaz;
                    break;
                    
                case 7:
                    orderSelector = p => p.BrojMjesta;
                    break;
                    
                case 8:
                    orderSelector = p => p.CijenaPoSatu;
                    break;

                case 9:
                    orderSelector = p => p.StranaCesteUlaz;
                    break;

            }
            if (orderSelector != null)
            {
                if (secondOrderSelector != null)
                {
                    query = ascending ?
                        query.OrderBy(orderSelector).ThenBy(secondOrderSelector) :
                        query.OrderByDescending(orderSelector).ThenByDescending(secondOrderSelector);
                }
                else
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
