using BaseLibrary;
using Clanstvo.Domain.Models;
namespace Clanstvo.Repositories;

/// <summary>
/// Facade interface for a Role repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam>
public interface IClanarinaRepository
    : IRepository<int, Clanarina>
{
    /// <summary>
    /// Gets all clanarine wiht placenost == false
    /// </summary>
    /// <returns><c>IEnumerable</c> of entities</returns>
    Result<IEnumerable<Clanarina>> GetAllNeplacene();
}