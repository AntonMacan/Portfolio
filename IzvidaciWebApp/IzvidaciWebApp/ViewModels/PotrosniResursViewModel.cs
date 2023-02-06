namespace IzvidaciWebApp.ViewModels;

public class PotrosniResursViewModel : ResursViewModel
{
    private DateOnly _rokTrajanja;
    
    public DateTime RokTrajanjaEdit { get; set; }
    
    public DateOnly RokTrajanja { get => _rokTrajanja; set => _rokTrajanja = value; }
}