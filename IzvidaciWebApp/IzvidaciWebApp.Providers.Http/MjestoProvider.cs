
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

public class MjestoProvider : IMjestoProvider
{
    private readonly MjestoProviderOptions _options;
    private readonly HttpClient _httpClient;

    public MjestoProvider(MjestoProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("MjestoOptions");
    }

    public async Task<Result<Mjesto>> Get(int id)
    {
        var mjestoDTO = (await _httpClient.GetFromJsonAsync<MjestoDto>($"/api/Mjesta/pbrMjesta?pbr={id}"));
        if (mjestoDTO is not null)
        {
            var mjesto = DtoMapping.ToDomain(mjestoDTO);
            return Results.OnSuccess<Mjesto>(mjesto);
        }
        return Results.OnFailure<Mjesto>("Mjesto ne postoji");
    }


    public async Task<Result<IEnumerable<Mjesto>>> GetAll()
    {
        var mjestaDTO = (await _httpClient.GetFromJsonAsync<IEnumerable<MjestoDto>>($"/api/Mjesta"));

        if (mjestaDTO is not null)
        {
            var mjesto = mjestaDTO.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess<IEnumerable<Mjesto>>(mjesto);
        }
        return Results.OnFailure<IEnumerable<Mjesto>>("Mjesta ne postoje.");
    }

    public async Task<Result> Delete(int id)
    {
        String str = $"api/Mjesta/pbrMjesta?pbr={id}";
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(Mjesto mjesto)
    {
        String str = "api/Mjesta/";
        var json = JsonConvert.SerializeObject(mjesto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> Edit(int id, Mjesto mjesto)
    {
        String str = $"api/Mjesta/{id}";
        var json = JsonConvert.SerializeObject(mjesto);
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

