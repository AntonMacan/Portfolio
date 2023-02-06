using BaseLibrary;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers;

public interface IClanProvider
{
    Task<Result<Clan>> Get(int id);
    Task<Result<IEnumerable<Clan>>> GetAll();
    Task<Result> Delete(int id);
    Task<Result> Create(Clan clan);
    Task<Result> Edit(int id, Clan clan);
}