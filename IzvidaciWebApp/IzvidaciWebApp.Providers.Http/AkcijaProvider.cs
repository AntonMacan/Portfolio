using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IzvidaciWebApp.Providers.Http;
public class AkcijaProvider : IAkcijaProvider
{
    private readonly AkcijaProviderOptions _options;
    private readonly HttpClient _httpClient;

    public AkcijaProvider(AkcijaProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("AkcijeOptions");
    }

    public async Task<Result<Akcija>> Get(int id)
    {
        var akcijaDto = (await _httpClient.GetFromJsonAsync<AkcijaDto>($"/api/Akcije/{id}"));
        if (akcijaDto is not null)
        {
            var akcija = DtoMapping.ToDomain(akcijaDto);
            return Results.OnSuccess<IzvidaciWebApp.Domain.Models.Akcija>(akcija);
        }
        return Results.OnFailure<IzvidaciWebApp.Domain.Models.Akcija>("Akcija ne postoji");
    }

    public async Task<Result<IEnumerable<Akcija>>> GetAll()
    {
        var akcijaDto = (await _httpClient.GetFromJsonAsync<IEnumerable<AkcijaDto>>($"/api/Akcije"));

        if (akcijaDto is not null)
        {
            var akcija = akcijaDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess<IEnumerable<IzvidaciWebApp.Domain.Models.Akcija>>(akcija);
        }
        return Results.OnFailure<IEnumerable<IzvidaciWebApp.Domain.Models.Akcija>>("Akcije ne postoji");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/Akcije/" + id;
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(Akcija akcija)
    {
        String str = "api/Akcije/";
        var json = JsonConvert.SerializeObject(akcija);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> Edit(int id, Akcija akcija)
    {
        String str = "api/Akcije/" + id;
        var json = JsonConvert.SerializeObject(akcija);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PutAsync(str, data);
        if (!response.IsCompleted)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

  
}
