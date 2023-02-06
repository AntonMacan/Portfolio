using Clanstvo.Commons;
using Clanstvo.DataAccess.SqlServer.Data;
using Clanstvo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BaseLibrary;
using System.Collections.Generic;

namespace Clanstvo.Repositories.SqlServer;

public class ClanRepository : IClanRepository
{
    private readonly ClanstvoContext _dbContext;

    public ClanRepository(ClanstvoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Clan model)
    {
        try
        {
            return _dbContext.Clan
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

            var model = _dbContext.Clan
                                  .AsNoTracking()
                                  .FirstOrDefault(clan => clan.Id.Equals(id));
            return model is not null;
        } catch
        {
            return false;
        }
    }

    public Result<Clan> Get(int id)
    {
        try
        {
            var model = _dbContext.Clan
                .AsNoTracking()
                .FirstOrDefault(clan => clan.Id.Equals(id))?
                .ToDomain();

            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Clan>($"No clan with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Clan>(e);
        }
    }

    public Result<Clan> GetNijePlatio(int id)
    {
        try
        {
            var model = _dbContext.Clan
                //.Where(clan => clan.Clanarina.Any(clanarina => clanarina.Placenost == false))
                //.Include(clan => clan.Clanarina.Where(clanarina => clanarina.Placenost == false))
                .Include(clan => clan.Clanarina)
                .AsNoTracking()
                .FirstOrDefault(clan => clan.Id.Equals(id))?
                .ToDomain(); // give me the first or null; substitute for .Where()
            // single or default throws an exception if more than one element meets the criteria

            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Clan>();
        }
        catch (Exception e)
        {
            return Results.OnException<Clan>(e);
        }

    }

    public Result<Clan> GetRangZasluga(int id)
    {
        try
        {
            var model = _dbContext.Clan
                .Include(clan => clan.ClanRangZasluga)
                .ThenInclude(clanRangZasluga => clanRangZasluga.RangZasluga)
                .AsNoTracking()
                .FirstOrDefault(clan => clan.Id.Equals(id))?
                .ToDomain(); // give me the first or null; substitute for .Where()
            // single or default throws an exception if more than one element meets the criteria

            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Clan>();
        }
        catch (Exception e)
        {
            return Results.OnException<Clan>(e);
        }

    }


    public Result<Clan> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Clan
                .Include(clan => clan.ClanRangZasluga)
                .ThenInclude(clanRangZasluga => clanRangZasluga.RangZasluga)
                .Include(clan => clan.ClanRangStarost)
                .ThenInclude(clanRangStarost => clanRangStarost.RangStarost)
                .Include(clan => clan.Clanarina)
                .AsNoTracking()
                .FirstOrDefault(clan => clan.Id.Equals(id))?
                .ToDomain(); // give me the first or null; substitute for .Where()
            // single or default throws an exception if more than one element meets the criteria

            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Clan>();
        }
        catch (Exception e)
        {
            return Results.OnException<Clan>(e);
        }

    }

    public Result<IEnumerable<Clan>> GetAll()
    {
        try
        {
            var models = _dbContext.Clan
                .AsNoTracking()
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clan>>(e);
        }
    }

    public Result<IEnumerable<Clan>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Clan
                .Include(clan => clan.ClanRangZasluga)
                .ThenInclude(clanRangZasluga => clanRangZasluga.RangZasluga)
                .Include(clan => clan.ClanRangStarost)
                .ThenInclude(clanRangStarost => clanRangStarost.RangStarost)
                .Include(clan => clan.Clanarina)
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clan>>(e);
        }
        
    }

    public Result<IEnumerable<Clan>> GetSviNisuPlatili()
    {
        try
        {
            var models = _dbContext.Clan
                //.Where(clan => clan.Clanarina.Any(clanarina => clanarina.Placenost == false))
                //.Include(clan => clan.Clanarina.Where(clanarina => clanarina.Placenost == false))
                .Include(clan => clan.Clanarina)
                .AsNoTracking()
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clan>>(e);
        }
    }

    public Result<IEnumerable<Clan>> GetNisuPlatili(int[] ids)
    {
        try
        {
            var models = _dbContext.Clan
                .Where(clan => ids.Contains(clan.Id))
                //.Where(clan => clan.Clanarina.Any(clanarina => clanarina.Placenost == false))
                //.Include(clan => clan.Clanarina.Where(clanarina => clanarina.Placenost == false))
                .Include(clan => clan.Clanarina)
                .AsNoTracking()
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clan>>(e);
        }
    }

    public Result<IEnumerable<Clan>> GetSviRangoviZasluga()
    {
        try
        {
            var models = _dbContext.Clan
                .Include(clan => clan.ClanRangZasluga)
                .ThenInclude(clanRangZasluga => clanRangZasluga.RangZasluga)
                .AsNoTracking()
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clan>>(e);
        }
    }

    public Result<IEnumerable<Clan>> GetRangoviZasluga(int[] ids)
    {
        try
        {
            var models = _dbContext.Clan
                .Where(clan => ids.Contains(clan.Id))
                .Include(clan => clan.ClanRangZasluga)
                .ThenInclude(clanRangZasluga => clanRangZasluga.RangZasluga)
                .AsNoTracking()
                .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Clan>>(e);
        }
    }

    public Result Insert(Clan model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Clan.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Clan
                .AsNoTracking()
                .FirstOrDefault(clan => clan.Id.Equals(id));

            if (model is not null)
            {
                _dbContext.Clan.Remove(model);

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

    public Result Update(Clan model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Clan.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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

    public Result UpdateAggregate(Clan model)
    {
       try{
            _dbContext.ChangeTracker.Clear();

            var dbModel  = _dbContext.Clan
                    .Include(clan => clan.ClanRangZasluga)
                    .ThenInclude(clanRangZasluga => clanRangZasluga.RangZasluga)
                    .Include(clan => clan.ClanRangStarost)
                    .ThenInclude(clanRangStarost => clanRangStarost.RangStarost)
                    .Include(clan => clan.Clanarina)
                    .FirstOrDefault(_ => _.Id == model.Id);
            if (dbModel == null)
                return Results.OnFailure($"Clan with id {model.Id} not found.");

            dbModel.Ime = model.Ime;
            dbModel.Prezime = model.Prezime;
            dbModel.DatumRodenja = model.DatumRodenja;
            dbModel.DatumMarama = model.DatumMarama;
            dbModel.Slika = model.Slika;
            dbModel.Adresa = model.Adresa;
            dbModel.ImaMaramu = model.ImaMaramu;
            dbModel.MjestoMarama = model.MjestoMarama;
            
            foreach (var dodjelaZasluga in model.DodjeleZasluga)
            {
                var clanZaslugaToUpdate =
                    dbModel.ClanRangZasluga
                           .FirstOrDefault(pr => pr.ClanId.Equals(model.Id) && pr.RangZaslugaId.Equals(dodjelaZasluga.RangZasluga.Id));
                if (clanZaslugaToUpdate != null)
                {
                    clanZaslugaToUpdate.Datum = dodjelaZasluga.Datum;
                }
                else
                {
                    dbModel.ClanRangZasluga.Add(dodjelaZasluga.ToDbModel(model.Id));
                }
            }
            
            dbModel.ClanRangZasluga
                   .Where(pr => !model.DodjeleZasluga.Any(_ => _.RangZasluga.Id == pr.RangZaslugaId))
                   .ToList()
                   .ForEach( zasluga =>{
                       dbModel.ClanRangZasluga.Remove(zasluga);
                   });
            
            foreach (var dodjelaStarost  in model.DodjeleStarost)
            {
                var clanStarostToUpdate =
                    dbModel.ClanRangStarost
                        .FirstOrDefault(pr => pr.ClanId.Equals(model.Id) && pr.RangStarostId.Equals(dodjelaStarost.RangStarost.Id));
                if (clanStarostToUpdate != null)
                {
                    clanStarostToUpdate.Datum = dodjelaStarost.Datum;
                }
                else
                {
                    dbModel.ClanRangStarost.Add(dodjelaStarost.ToDbModel(model.Id));
                }
            }
            
            dbModel.ClanRangStarost
                .Where(pr => !model.DodjeleStarost.Any(_ => _.RangStarost.Id == pr.RangStarostId))
                .ToList()
                .ForEach( starost =>{
                    dbModel.ClanRangStarost.Remove(starost);
                });

            

           foreach(var clanarina in model.Clanarina)
            {
                var clanarinaToUpdate = dbModel.Clanarina
                    .FirstOrDefault(pr => pr.ClanId.Equals(model.Id) && pr.Id.Equals(clanarina.Id));
                if (clanarinaToUpdate != null)
                {
                    clanarinaToUpdate.Placenost = clanarina.Placenost;
                    clanarinaToUpdate.Iznos = clanarina.Iznos;
                    clanarinaToUpdate.Godina = clanarina.Godina;
                    clanarinaToUpdate.ClanId = clanarina.ClanId;
                    clanarinaToUpdate.Datum = clanarina.Datum;
                }
                else
                {
                    dbModel.Clanarina.Add(clanarina.ToDbModel());
                }
            }

            dbModel.Clanarina
                 .Where(pr => !model.Clanarina.Any(_ => _.Id == pr.Id))
                 .ToList()
                 .ForEach(clanarina =>
                 {
                     dbModel.Clanarina.Remove(clanarina);
                 });

            _dbContext.Clan
                      .Update(dbModel);

            var isSuccess = _dbContext.SaveChanges() > 0;
            _dbContext.ChangeTracker.Clear();
            return isSuccess
                ? Results.OnSuccess()
                : Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }
}