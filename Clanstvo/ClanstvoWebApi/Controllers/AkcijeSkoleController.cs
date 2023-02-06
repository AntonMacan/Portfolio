using BaseLibrary;
using Microsoft.AspNetCore.Mvc;
using Clanstvo.Providers;
using Clanstvo.Providers.Http.DTOs;
using ClanstvoWebApi.DTOs;
using DtoMapping = ClanstvoWebApi.DTOs.DtoMapping;

namespace ClanstvoWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AkcijeSkoleController : ControllerBase
{
    private readonly IAkcijeSkoleProvider _akcijeSkoleProvider;

    public AkcijeSkoleController(IAkcijeSkoleProvider akcijeSkoleProvider)
    {
        _akcijeSkoleProvider = akcijeSkoleProvider;
    }

    [HttpGet("Akcije")]
    public ActionResult<IEnumerable<AkcijaSudionik>> GetAkcijeSudionika(int id)
    {
        var akcijaResult = _akcijeSkoleProvider.GetAkcijeClana(id).Result
            .Map(a => a.Select(akcija => akcija.ToDto()));

        Console.WriteLine(akcijaResult.Data);

        return akcijaResult
            ? Ok(akcijaResult.Data)
            : Problem(akcijaResult.Message, statusCode: 404);
    }

    [HttpGet("Skole")]
    public ActionResult<IEnumerable<SkolaSudionik>> GetSkoleSudionika(int id)
    {
        var skolaResult = _akcijeSkoleProvider.GetSkoleClana(id).Result
            .Map(s => s.Select(skola => skola.ToDto()));

        Console.WriteLine(skolaResult.Data);

        return skolaResult
            ? Ok(skolaResult.Data)
            : Problem(skolaResult.Message, statusCode: 404);
    }


}