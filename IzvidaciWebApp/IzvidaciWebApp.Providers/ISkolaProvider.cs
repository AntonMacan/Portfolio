using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers;
public interface ISkolaProvider
{
    Task<Result<Skola>> GetSkolaEdukacije(int id);
    Task<Result<IEnumerable<Skola>>> GetAll();
    Task<Result<Edukacija>> GetEdukacija(int id);
    Task<Result> DolaziNaEdukaciju(int id, PolaznikNaEdukaciji prijavljen);
    Task<Result> EditSkola(Skola skola);
    Task<Result<Skola>> GetSkola(int id);
    Task<Result> DeleteSkola(int id);
    Task<Result> CreateSkola(Skola skola);
    Task<Result> CreateEdukacija(int idSkola, Edukacija edukacija);
    Task<Result> PrijaviPolaznika(int id, PrijavljeniClanNaEdukaciji polaznik);
    Task<Result> PrijaviPredavaca(int id, PredavacNaEdukaciji predavac);
    Task<Result<Edukacija>> GetEdukacijaBasic(int id);
    Task<Result> EditEdukacija(Edukacija edukacija);
    Task<Result> OdjaviClana(int idEdukacija, int idClan);
    Task<Result> DeleteEdukacija(int id);
    Task<Result> OdjaviPredavaca(int idEdukacija, int idClan);
}
