using System.Net;
using System.Net.Http.Json;
using System.Text;
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;

namespace IzvidaciWebApp.Providers.Http;

public class ClanarinaProvider : IClanarineProvider
{
    private readonly ClanarinaProviderOptions _options;
    private readonly HttpClient _httpClient;

    public ClanarinaProvider(ClanarinaProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("ClanarinaOptions");
    }

    public async Task<Result<Clanarina>> Get(int id)
    {
        var clanarinaDto = (await _httpClient.GetFromJsonAsync<ClanarinaDTO>($"/api/Clanarina/{id}"));
        if (clanarinaDto is not null)
        {
            var clanarina = DtoMapping.ToDomain(clanarinaDto);
            return Results.OnSuccess<Clanarina>(clanarina);
        }
        return Results.OnFailure<Clanarina>("Clanarina ne postoji");
    }

    public async Task<Result<IEnumerable<Clanarina>>> GetAll()
    {
        var clanarineDto = (await _httpClient.GetFromJsonAsync<IEnumerable<ClanarinaDTO>>($"/api/Clanarina"));

        if (clanarineDto is not null)
        {
            var clanarina = clanarineDto.Select(c => DtoMapping.ToDomain(c));
            return Results.OnSuccess<IEnumerable<Clanarina>>(clanarina);
        }
        return Results.OnFailure<IEnumerable<Clanarina>>("Clanarine ne postoje");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/Clanarina/" + id + "";
        var response = _httpClient.DeleteAsync(str);
        if (!response.Result.IsSuccessStatusCode)
        {
            return Results.OnFailure("Greska pri brisanju clanarine");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(Clanarina clanarina)
    {
        String str = "api/Clanarina/";
        var json = JsonConvert.SerializeObject(clanarina);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Results.OnFailure("Greska pri dodavanju clanarine");
        }

        return Results.OnSuccess("Uspjesno dodano");
    }

    public async Task<Result> Edit(int id, Clanarina clanarina)
    {
        String str = "api/Clanarina/" + id;
        var json = JsonConvert.SerializeObject(clanarina);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(str, data);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Results.OnFailure("Neuspjesno uredivanje clanarina");
        }
        return Results.OnSuccess("Uspjesno uredivanje clanarina");
    }
}