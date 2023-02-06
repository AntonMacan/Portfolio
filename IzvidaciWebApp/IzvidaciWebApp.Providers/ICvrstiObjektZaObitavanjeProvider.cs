using BaseLibrary;
using IzvidaciWebApp.Domain.Models;

namespace IzvidaciWebApp.Providers
{
    public interface ICvrstiObjektZaObitavanjeProvider
    {
        Task<Result<CvrstiObjektZaObitavanje>> Get(int id);
        Task<Result<IEnumerable<CvrstiObjektZaObitavanje>>> GetAll();
        Task<Result> Delete(int id);
        Task<Result> Create(CvrstiObjektZaObitavanje objekt);
        Task<Result> Edit(int id, CvrstiObjektZaObitavanje objekt);
    }
}