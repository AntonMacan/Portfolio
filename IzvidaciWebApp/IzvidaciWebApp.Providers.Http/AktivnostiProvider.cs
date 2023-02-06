using System.Collections;
using System.Net.Http.Json;
using System.Text;
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;

namespace IzvidaciWebApp.Providers.Http;

public class AktivnostiProvider : IAktivnostProvider
{
    private readonly AkcijaProviderOptions _options;
    private readonly HttpClient _httpClient;

    public AktivnostiProvider(AkcijaProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("AkcijeOptions");
    }

    public async Task<Result<Aktivnost>> Get(int id)
    {
        var aktivnostDto = (await _httpClient.GetFromJsonAsync<AktivnostDto>($"/api/Aktivnosti/{id}"));
        if (aktivnostDto is not null)
        {
            var aktivnost = DtoMapping.ToDomain(aktivnostDto);
            return Results.OnSuccess<IzvidaciWebApp.Domain.Models.Aktivnost>(aktivnost);
        }
        return Results.OnFailure<IzvidaciWebApp.Domain.Models.Aktivnost>("Aktivnost ne postoji");
    }

    public async Task<Result<IEnumerable<Aktivnost>>> GetAll()
    {
        var aktivnostDto = (await _httpClient.GetFromJsonAsync<IEnumerable<AktivnostDto>>($"/api/Aktivnosti"));

        if (aktivnostDto is not null)
        {
            var aktivnost = aktivnostDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess<IEnumerable<IzvidaciWebApp.Domain.Models.Aktivnost>>(aktivnost);
        }
        return Results.OnFailure<IEnumerable<IzvidaciWebApp.Domain.Models.Aktivnost>>("Aktivnosti ne postoji");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/Aktivnosti/" + id;
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(Aktivnost aktivnost)
    {
        String str = "api/Aktivnosti/";
        var json = JsonConvert.SerializeObject(aktivnost);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> Edit(int id, Aktivnost aktivnost)
    {
        String str = "api/Aktivnosti/" + id;
        var json = JsonConvert.SerializeObject(aktivnost);
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