namespace IzvidaciWebApp.Domain.Models;

public class PotrosniResurs : Resurs
{

    private DateTime _rokTrajanja;
    
    public PotrosniResurs(int id, string naziv, string? napomena, DateTime datumNabave, int idUdruge,  string udruga, int idProstor, string prostor, DateTime rokTrajanja) : base(naziv, napomena, datumNabave, idUdruge, udruga, idProstor, prostor, id)
    {
        if (rokTrajanja == DateTime.MinValue)
        {
            throw new ArgumentException($"'{nameof(rokTrajanja)}' cannot be null or empty.", nameof(rokTrajanja));
        }

        _rokTrajanja = rokTrajanja;
    }
    
    public DateTime RokTrajanja
    {
        get => _rokTrajanja;
        set => _rokTrajanja = value;
    }


}