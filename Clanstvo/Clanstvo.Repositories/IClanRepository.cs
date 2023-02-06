using BaseLibrary;
using Clanstvo.Domain.Models;
using System;

namespace Clanstvo.Repositories;

/// <summary>
/// Facade interface for a Person repository
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDomainModel"></typeparam
public interface IClanRepository
    : IRepository<int, Clan>,
      IAggregateRepository<int, Clan>,
    IIzvedniClanRepository<int, Clan>
{
}