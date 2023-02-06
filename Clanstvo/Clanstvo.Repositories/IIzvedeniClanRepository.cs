using BaseLibrary;
using Clanstvo.Domain;

namespace Clanstvo.Repositories
{
    /// <summary>
    /// Interface extension for repositories working over aggregates
    /// </summary>
    /// <typeparam name="TKey">Type of key in aggregate root</typeparam>
    /// <typeparam name="TAggregate">Type of aggregate root</typeparam>
    public interface IIzvedniClanRepository<TKey, TAggregate> where TAggregate : AggregateRoot<TKey>
    {
        /// <summary>
        /// Gets an aggregate with given key/id
        /// </summary>
        /// <param name="id">Aggregate root id</param>
        /// <returns>Option of <c>TAggregate</c></returns>
        //Result<TAggregate> GetAggregate(TKey id);

        Result<TAggregate> GetNijePlatio(TKey id);

        Result<TAggregate> GetRangZasluga(TKey id);
        /// <summary>
        /// Gets all aggregates
        /// </summary>
        /// <returns><c>IEnumerable</c> of <c>TAggregate</c></returns>
        //Result<IEnumerable<TAggregate>> GetAllAggregates();

        Result<IEnumerable<TAggregate>> GetSviNisuPlatili();

        Result<IEnumerable<TAggregate>> GetNisuPlatili(TKey[] ids);

        Result<IEnumerable<TAggregate>> GetSviRangoviZasluga();

        Result<IEnumerable<TAggregate>> GetRangoviZasluga(TKey[] ids);
        /// <summary>
        /// Updates the entire aggregate
        /// </summary>
        /// <param name="model">Aggregate object</param>
        /// <returns><c>true</c> on success, <c>false</c> on failure</returns>
        //Result UpdateAggregate(TAggregate model);
    }
}
