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
public class SkolaProvider : ISkolaProvider
{
    private readonly AkcijaProviderOptions _options;
    private readonly HttpClient _httpClient;

    public SkolaProvider(AkcijaProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("AkcijeOptions");
    }

    public async Task<Result<Skola>> GetSkolaEdukacije(int id)
    {
        var result = await _httpClient.GetFromJsonAsync<SkolaEdukacijeDto>($"/api/Skole/SkoleAggregate/{id}");

        if (result is not null)
        {
            var skolaEdukacije = DtoMapping.ToDomainAggregate(result);
            return Results.OnSuccess<Skola>(skolaEdukacije);
        }
        return Results.OnFailure<Skola>("Skola ne postoji");
    }

    public async Task<Result<IEnumerable<Skola>>> GetAll()
    {
        var result = (await _httpClient.GetFromJsonAsync<IEnumerable<SkolaDto>>($"/api/Skole"));

        if (result is not null)
        {
            var skole = result.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess<IEnumerable<Skola>>(skole);
        }
        return Results.OnFailure<IEnumerable<Skola>>("Skole ne postoji");
    }

    public async Task<Result<Edukacija>> GetEdukacija(int id)
    {
        var result = await _httpClient.GetFromJsonAsync<EdukacijaAggregateDto>($"/api/Edukacija/EdukacijaAggregate/{id}");

        if (result is not null)
        {
            var edukacija = DtoMapping.toDomainAggregate(result);
            return Results.OnSuccess<Edukacija>(edukacija);
        }
        return Results.OnFailure<Edukacija>("Edukacija ne postoji");
    }

    public async Task<Result> DolaziNaEdukaciju(int id, PolaznikNaEdukaciji polaznik)
    {
        String str = $"/api/Edukacija/DolaziNaEdukaciju/{id}";
        var json = JsonConvert.SerializeObject(polaznik.ToDto());
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PostAsync(str, data);
        if (!response.IsCompleted)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> EditSkola(Skola skola)
    {
        String str = $"api/Skole/{skola.IdSkole}";
        var json = JsonConvert.SerializeObject(skola.ToDto());
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PutAsync(str, data);
        if (!response.IsCompletedSuccessfully)
        {
            Console.WriteLine("Greska");
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result<Skola>> GetSkola(int id)
    {
        var result = await _httpClient.GetFromJsonAsync<SkolaDto>($"/api/Skole/{id}");

        if (result is not null)
        {
            var skolaDomain = DtoMapping.ToDomain(result);
            return Results.OnSuccess<Skola>(skolaDomain);
        }
        return Results.OnFailure<Skola>("Skola ne postoji");
    }

    public async Task<Result<Edukacija>> GetEdukacijaBasic(int id)
    {
        var result = await _httpClient.GetFromJsonAsync<EdukacijaDto>($"/api/Edukacija/{id}");

        if (result is not null)
        {
            var edukacijaDomain = DtoMapping.toDomain(result);
            return Results.OnSuccess<Edukacija>(edukacijaDomain);
        }
        return Results.OnFailure<Edukacija>("Edukacija ne postoji");
    }

    public async Task<Result> EditEdukacija(Edukacija edukacija)
    {
        String str = $"api/Edukacija/{edukacija.Id}";
        var json = JsonConvert.SerializeObject(edukacija.ToDto());
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PutAsync(str, data);
        if (!response.IsCompletedSuccessfully)
        {
            Console.WriteLine("Greska");
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> DeleteSkola(int id)
    {
        String str = $"api/Skole/{id}";
        var response = _httpClient.DeleteAsync(str);
        if (!response.IsCompletedSuccessfully)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> CreateSkola(Skola skola)
    {
        String str = "api/Skole";
        var json = JsonConvert.SerializeObject(skola.ToDto());
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> CreateEdukacija(int idSkola, Edukacija edukacija)
    {
        String str = $"api/Skole/DodajEdukaciju/{idSkola}";
        var json = JsonConvert.SerializeObject(edukacija.ToDto());
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> PrijaviPolaznika(int id, PrijavljeniClanNaEdukaciji polaznik)
    {
        String str = $"/api/Edukacija/PrijaviPolaznika/{id}";
        var json = JsonConvert.SerializeObject(polaznik.ToDto());
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PostAsync(str, data);
        if (!response.IsCompletedSuccessfully)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> PrijaviPredavaca(int id, PredavacNaEdukaciji predavac)
    {
        String str = $"/api/Edukacija/DodajPredavaca/{id}";
        var json = JsonConvert.SerializeObject(predavac.ToDto());
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PostAsync(str, data);
        if (!response.IsCompletedSuccessfully)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> OdjaviClana(int idEdukacija, int idClan )
    {
        String str = $"/api/Edukacija/OdjaviPolaznika/{idEdukacija}?polaznikId={idClan}";
        var data = new StringContent("", Encoding.UTF8, "application/json");
        var response = _httpClient.PostAsync(str, data);
        if (!response.IsCompletedSuccessfully)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> DeleteEdukacija(int id)
    {
        String str = $"api/Edukacija/{id}";
        var response = _httpClient.DeleteAsync(str);
        if (!response.IsCompletedSuccessfully)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> OdjaviPredavaca(int idEdukacija, int idClan)
    {
        String str = $"/api/Edukacija/UkloniPredavaca/{idEdukacija}?predavacId={idClan}";
        var data = new StringContent("", Encoding.UTF8, "application/json");
        var response = _httpClient.PostAsync(str, data);
        if (!response.IsCompletedSuccessfully)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }
}
