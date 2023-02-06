using System.Net.Http.Json;
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;

namespace IzvidaciWebApp.Providers.Http;

public class ProstoriProvider : IProstoriProvider
{
    private readonly UdrugaProviderOptions _options;
    private readonly HttpClient _httpClient;
    
    public ProstoriProvider(UdrugaProviderOptions options,IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("UdrugeOptions");
    }

    public async Task<Result<IEnumerable<Prostori>>> GetAll()
    {  
        var prostorDto = (await _httpClient.GetFromJsonAsync<IEnumerable<ProstoriDto>>($"/api/Prostori"));

        if (prostorDto is not null)
        {
            var prostor = prostorDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess(prostor);
        }
        return Results.OnFailure<IEnumerable<Prostori>>("prostor ne postoji");
    }
}

