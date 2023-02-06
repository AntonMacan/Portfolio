using BaseLibrary;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers;

public interface IResursiProvider
{
    
    Task<Result<TrajniResurs>> GetTrajni(int id);
    Task<Result<PotrosniResurs>> GetPotrosni(int id);
    Task<Result<IEnumerable<Resurs>>> GetAll();
    Task<Result<IEnumerable<PotrosniResurs>>> GetAllPotrosni();
    Task<Result<IEnumerable<TrajniResurs>>> GetAllTrajni();
    Task<Result> Delete(int id);
    Task<Result> CreatePotrosni(PotrosniResurs resurs);
    Task<Result> CreateTrajni(TrajniResurs resurs);
    Task<Result> EditTrajni(int id, TrajniResurs resurs);
    Task<Result> EditPotrosni(int id, PotrosniResurs resurs);
    
}