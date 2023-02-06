namespace IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class VoditeljiUdrugeDto
{
    public int IdUdruge { get; set; }
    public int IdClan { get; set; }
    public string Pozicija { get; set; } = string.Empty;
    public DateTime? NaPozicijiDo { get; set; } 

}

public static partial class DtoMapping
{
    public static VoditeljiUdrugeDto ToDto(this VoditeljiUdruge voditeljUdruge)
    {
        return new VoditeljiUdrugeDto()
        {
            IdUdruge = voditeljUdruge.IdUdruge,
            IdClan = voditeljUdruge.IdClan,
            Pozicija = voditeljUdruge.Pozicija,
            NaPozicijiDo = voditeljUdruge.NaPozicijiDo,
        };
    }
    public static VoditeljiUdruge ToDomain(this VoditeljiUdrugeDto voditeljUdruge)
    {
        return new VoditeljiUdruge(voditeljUdruge.IdUdruge, voditeljUdruge.IdClan, voditeljUdruge.Pozicija, voditeljUdruge.NaPozicijiDo);
    }
}
