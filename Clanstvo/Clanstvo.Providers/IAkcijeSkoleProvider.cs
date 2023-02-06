using BaseLibrary;
using Clanstvo.Domain.Models;

namespace Clanstvo.Providers;

public interface IAkcijeSkoleProvider
{
    public Task<Result<IEnumerable<Skola>>> GetSkoleClana(int id);
    public Task<Result<IEnumerable<Akcija>>> GetAkcijeClana(int id);
}