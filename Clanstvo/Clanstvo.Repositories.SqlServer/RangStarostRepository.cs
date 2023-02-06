using Clanstvo.DataAccess.SqlServer.Data;
using Clanstvo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BaseLibrary;

namespace Clanstvo.Repositories.SqlServer;

public class RangStarostRepository : IRangStarostRepository
{
    private readonly ClanstvoContext _dbContext;

    public RangStarostRepository(ClanstvoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(RangStarost model)
    {
        try
        {
            return _dbContext.RangStarost
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
            return _dbContext.RangStarost
                .AsNoTracking()
                .FirstOrDefault(starost => starost.Id.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<RangStarost> Get(int id)
    {
        try
        {
            var starost = _dbContext.RangStarost
                .AsNoTracking()
                .FirstOrDefault(starost => starost.Id.Equals(id))?
                .ToDomain();

            return starost is not null
                ? Results.OnSuccess(starost)
                : Results.OnFailure<RangStarost>($"No rang starost with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<RangStarost>(e);
        }
    }


    public Result<IEnumerable<RangStarost>> GetAll()
    {
        try
        {
            var starost = 
                _dbContext.RangStarost
                    .AsNoTracking()
                    .Select(Mapping.ToDomain);
            return Results.OnSuccess(starost);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<RangStarost>>(e);
        }
    }


    public Result Insert(RangStarost model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.RangStarost.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.RangStarost
                .AsNoTracking()
                .FirstOrDefault(starost => starost.Id.Equals(id));
            if (model is not null)
            {
                _dbContext.RangStarost.Remove(model);

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

    public Result Update(RangStarost model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.RangStarost.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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
