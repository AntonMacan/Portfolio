using System.Net;
using System.Net.Http.Json;
using System.Text;
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;

namespace IzvidaciWebApp.Providers.Http;

public class ClanProvider : IClanProvider
{
    private readonly ClanProviderOptions _options;
    private readonly HttpClient _httpClient;

    public ClanProvider(ClanProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("ClanOptions"); 
    }

    public async Task<Result<Clan>> Get(int id)
    {
        var clanDto = (await _httpClient.GetFromJsonAsync<ClanDto>($"/api/Clan/{id}"));
        if (clanDto is not null)
        {
            var clan = DtoMapping.ToDomainClan(clanDto);
            return Results.OnSuccess<Clan>(clan);
        }
        return Results.OnFailure<Clan>("Clanarina ne postoji");
    }

    public async Task<Result<IEnumerable<Clan>>> GetAll()
    {
        var clanDto = (await _httpClient.GetFromJsonAsync<IEnumerable<ClanDto>>($"/api/Clan"));

        if (clanDto is not null)
        {
            var clan = clanDto.Select(c => DtoMapping.ToDomainClan(c));
            return Results.OnSuccess<IEnumerable<Clan>>(clan);
        }
        return Results.OnFailure<IEnumerable<Clan>>("Clanovi ne postoje");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/Clan/" + id + "";
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(Clan clan)
    {
        String str = "api/Clan/";
        var json = JsonConvert.SerializeObject(clan);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!(response.StatusCode == HttpStatusCode.Accepted))
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Edit(int id, Clan clan)
    {
        String str = "api/Clan/" + id;
        var json = JsonConvert.SerializeObject(clan);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PutAsync(str, data);
        if (!response.IsCompleted)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }
}