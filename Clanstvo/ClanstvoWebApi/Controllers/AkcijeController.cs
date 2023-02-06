using Microsoft.AspNetCore.Mvc;
using ClanstvoWebApi.DTOs;

namespace ClanstvoWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AkcijeController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public AkcijeController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AkcijeSkole");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Akcija>>> GetDidntPay()
    {
        var resursResults = await _httpClient.GetFromJsonAsync<IEnumerable<Akcija>>("api/Akcije");

        return Ok(resursResults);
    }
}