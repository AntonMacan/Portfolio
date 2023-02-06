using Clanstvo.DataAccess.SqlServer.Data;
using Clanstvo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BaseLibrary;

namespace Clanstvo.Repositories.SqlServer;

public class RangZaslugaRepository : IRangZaslugaRepository
{
    private readonly ClanstvoContext _dbContext;

    public RangZaslugaRepository(ClanstvoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(RangZasluga model)
    {
        try
        {
            return _dbContext.RangZasluga
                .AsNoTracking()
                .Contains(model.ToDbModel());
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Exists(int id)
    {
        try
        {
            return _dbContext.RangZasluga
                .AsNoTracking()
                .FirstOrDefault(zasluga => zasluga.Id.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<RangZasluga> Get(int id)
    {
        try
        {
            var role = _dbContext.RangZasluga
                .AsNoTracking()
                .FirstOrDefault(zasluga => zasluga.Id.Equals(id))?
                .ToDomain();

            return role is not null
                ? Results.OnSuccess(role)
                : Results.OnFailure<RangZasluga>($"No rang zalsuga with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<RangZasluga>(e);
        }

    }


    public Result<IEnumerable<RangZasluga>> GetAll()
    {
        try
        {
            var rangZasluge = 
                _dbContext.RangZasluga
                    .AsNoTracking()
                    .Select(Mapping.ToDomain);
            return Results.OnSuccess(rangZasluge);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<RangZasluga>>(e);
        }
    }


    public Result Insert(RangZasluga model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.RangZasluga.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                var isSuccess = _dbContext.SaveChanges() > 0;

                // every Add attaches the entity object and EF begins tracking
                // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
                _dbContext.Entry(dbModel).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                return isSuccess
                    ? Results.OnSuccess()
                    : Results.OnFailure();
            }

            return Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }

    public Result Remove(int id)
    {
        try
        {
            var model = _dbContext.RangZasluga
                .AsNoTracking()
                .FirstOrDefault(zasluga => zasluga.Id.Equals(id));
            if (model is not null)
            {
                _dbContext.RangZasluga.Remove(model);

                return _dbContext.SaveChanges() > 0
                    ? Results.OnSuccess()
                    : Results.OnFailure();
            }
            return Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }

    public Result Update(RangZasluga model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.RangZasluga.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                var isSuccess = _dbContext.SaveChanges() > 0;

                // every Update attaches the entity object and EF begins tracking
                // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
                _dbContext.Entry(dbModel).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                return isSuccess
                    ? Results.OnSuccess()
                    : Results.OnFailure();
            }

            return Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }
}
