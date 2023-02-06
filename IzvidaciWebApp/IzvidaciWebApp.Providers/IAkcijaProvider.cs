using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers;
public interface IAkcijaProvider
{
    Task<Result<Akcija>> Get(int id);
    Task<Result<IEnumerable<Akcija>>> GetAll();
    Task<Result> Delete(int id);
    Task<Result> Create(Akcija akcija);
    Task<Result> Edit(int id, Akcija akcija);
}
