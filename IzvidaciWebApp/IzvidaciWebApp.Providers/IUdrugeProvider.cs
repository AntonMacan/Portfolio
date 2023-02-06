using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers
{
    public interface IUdrugeProvider
    {

        Task<Result<Udruge>> Get(int id);
        Task<Result<IEnumerable<Udruge>>> GetAll();
        Task<Result> Delete(int id);
        Task<Result> Create(Udruge udruga);
        Task<Result> Edit(int id, Udruge udruga);
    }
}
