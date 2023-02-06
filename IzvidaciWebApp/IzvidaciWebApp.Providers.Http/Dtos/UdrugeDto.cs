namespace IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class UdrugeDto
{
    public int IdUdruge { get; set; }
    public string OIB { get; set; } = string.Empty;
    public string Naziv { get; set; } = string.Empty;
    public string Sjediste { get; set; } = string.Empty;
    public string BrMob { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;


}

public static partial class DtoMapping
{
    public static UdrugeDto ToDto(this Udruge udruga)
    {
        return new UdrugeDto()
        {
            IdUdruge = udruga.IdUdruge,
            OIB = udruga.OIB,
            Naziv = udruga.Naziv,
            Sjediste = udruga.Sjediste,
            BrMob = udruga.BrMob,
            Mail = udruga.Mail
        };
    }
    public static Udruge ToDomain(this UdrugeDto udruga)
    {
        return new Udruge(udruga.IdUdruge, udruga.OIB, udruga.Naziv, udruga.Sjediste, udruga.BrMob, udruga.Mail);
    }

    internal static object ToDomain(ClanDto c)
    {
        throw new NotImplementedException();
    }
}
