using Clanstvo.Domain.Models;
namespace Clanstvo.Repositories;

/// <summary>
/// Facade interface for a Role repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IRangStarostRepository
    : IRepository<int, RangStarost>
{
}