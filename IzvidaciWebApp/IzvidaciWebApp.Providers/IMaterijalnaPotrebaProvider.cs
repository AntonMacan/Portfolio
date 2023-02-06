using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers
{
    public interface IMaterijalnaPotrebaProvider
    {
        Task<Result<MaterijalnaPotreba>> Get(int id);
        Task<Result<IEnumerable<MaterijalnaPotreba>>> GetAll();
        Task<Result> Delete(int id);
        Task<Result> Create(MaterijalnaPotreba potreba);
        Task<Result> Edit(int id, MaterijalnaPotreba potreba);
    }
}
