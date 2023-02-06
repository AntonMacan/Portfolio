using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Providers;

public interface IRangStarostProvider
{
    Task<Result<RangStarost>> Get(int id);
    Task<Result<IEnumerable<RangStarost>>>GetAll();
    Task<Result> Delete(int id);
    Task<Result> Create(RangStarost rangStarost);
    Task<Result> Edit(int id,RangStarost rangStarost);
}
