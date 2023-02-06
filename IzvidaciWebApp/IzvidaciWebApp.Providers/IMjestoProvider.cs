using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers;

public interface IMjestoProvider
{
    Task<Result<Mjesto>> Get(int id);
    Task<Result<IEnumerable<Mjesto>>> GetAll();
    Task<Result> Delete(int id);
    Task<Result> Create(Mjesto mjesto);
    Task<Result> Edit(int id, Mjesto mjesto);
}
