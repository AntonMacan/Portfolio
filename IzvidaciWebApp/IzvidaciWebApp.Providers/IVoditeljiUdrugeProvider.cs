using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers
{
    public interface IVoditeljiUdrugeProvider
    {

        Task<Result<VoditeljiUdruge>> Get(int id);
        Task<Result<IEnumerable<VoditeljiUdruge>>> GetAll();
        Task<Result> Delete(int id);
        Task<Result> Create(VoditeljiUdruge voditeljUdruge);
        Task<Result> Edit(int id, VoditeljiUdruge voditeljUdruge);
    }
}
