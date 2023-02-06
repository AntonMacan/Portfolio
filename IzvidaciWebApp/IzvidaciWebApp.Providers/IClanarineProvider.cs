using BaseLibrary;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers;

public interface IClanarineProvider
{
    Task<Result<Clanarina>> Get(int id);
    Task<Result<IEnumerable<Clanarina>>> GetAll();
    Task<Result> Delete(int id);
    Task<Result> Create(Clanarina clanarina);
    Task<Result> Edit(int id, Clanarina clanarina);
}
