using Clanstvo.DataAccess.SqlServer.Data;
using Clanstvo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BaseLibrary;

namespace Clanstvo.Repositories.SqlServer;
public class ClanarinaRepository : IClanarinaRepository
{
    private readonly ClanstvoContext _dbContext;

    public ClanarinaRepository(ClanstvoContext dbContext)
    {
        _dbContext = dbContext;
    }
    public bool Exists(Clanarina model)
    {
        try
        {
            return _dbContext.Clanarina
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
            var model = _dbContext.Clanarina
                .AsNoTracking()
                .FirstOrDefault(clanarina => clanarina.Id.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Clanarina> Get(int id)
    {
        try{
        var model = _dbContext.Clanarina
                              .AsNoTracking()
                              .FirstOrDefault(clanarina => clanarina.Id.Equals(id))?
                              .ToDomain();

        return model is not null
            ? Results.OnSuccess(model)
            : Results.OnFailure<Clanarina>($"No clanarina with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Clanarina>(e);
        }
    }

    public Result<IEnumerable<Clanarina>> GetAll()
    {
        try
        {
            var models = _dbContext.Clanarina
                .AsNoTracking()
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clanarina>>(e);
        }
    }

    public Result<IEnumerable<Clanarina>> GetAllNeplacene()
    {
        try
        {
            var models = _dbContext.Clanarina
                .Where(clanarina => clanarina.Placenost == false)
                .AsNoTracking()
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clanarina>>(e);
        }
    }

    public Result Insert(Clanarina model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Clanarina.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Clanarina
                .AsNoTracking()
                .FirstOrDefault(clanarina => clanarina.Id.Equals(id));

            if (model is not null)
            {
                _dbContext.Clanarina.Remove(model);

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

    public Result Update(Clanarina model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Clanarina.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


