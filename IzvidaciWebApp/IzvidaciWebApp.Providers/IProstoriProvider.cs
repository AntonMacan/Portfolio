using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Providers;

public interface IProstoriProvider
{
    Task<Result<IEnumerable<Prostori>>> GetAll();
}
