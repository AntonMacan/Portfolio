using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Providers;

public interface IRangZaslugaProvider
{
    Task<Result<RangZasluga>> Get(int id);
    Task<Result<IEnumerable<RangZasluga>>>GetAll();
    Task<Result> Delete(int id);
    Task<Result> Create(RangZasluga rangZasluga);
    Task<Result> Edit(int id,RangZasluga rangZasluga);
}
