using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Providers;

    public interface IAktivnostProvider
    {
        Task<Result<Aktivnost>> Get(int id);
        Task<Result<IEnumerable<Aktivnost>>> GetAll();
        Task<Result> Delete(int id);
        Task<Result> Create(Aktivnost aktivnost);
    Task<Result> Edit(int id, Aktivnost aktivnost);
}

