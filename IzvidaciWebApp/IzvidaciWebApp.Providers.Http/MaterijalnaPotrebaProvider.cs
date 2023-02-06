
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

public class MaterijalnaPotrebaProvider : IMaterijalnaPotrebaProvider
{
    private readonly MaterijalnaPotrebaProviderOptions _options;
    private readonly HttpClient _httpClient;

    public MaterijalnaPotrebaProvider(MaterijalnaPotrebaProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("MaterijalnaPotrebaOptions");
    }

    public async Task<Result<MaterijalnaPotreba>> Get(int id)
    {
        var mjestoDTO = (await _httpClient.GetFromJsonAsync<MaterijalnaPotrebaDto>($"/api/MaterijalnePotrebe/{id}"));
        if (mjestoDTO is not null)
        {
            var mjesto = DtoMapping.ToDomain(mjestoDTO);
            return Results.OnSuccess<MaterijalnaPotreba>(mjesto);
        }
        return Results.OnFailure<MaterijalnaPotreba>("Materijalna potreba ne postoji");
    }


    public async Task<Result<IEnumerable<MaterijalnaPotreba>>> GetAll()
    {
        var mjestaDTO = (await _httpClient.GetFromJsonAsync<IEnumerable<MaterijalnaPotrebaDto>>($"/api/MaterijalnePotrebe"));

        if (mjestaDTO is not null)
        {
            var mjesto = mjestaDTO.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess<IEnumerable<MaterijalnaPotreba>>(mjesto);
        }
        return Results.OnFailure<IEnumerable<MaterijalnaPotreba>>("Materijalne potrebe ne postoje.");
    }

    public async Task<Result> Delete(int id)
    {
        String str = $"api/MaterijalnePotrebe/idMaterijalnaPotreba?idMaterijalnaPotreba={id}";
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(MaterijalnaPotreba potreba)
    {
        String str = "api/MaterijalnePotrebe/";
        var json = JsonConvert.SerializeObject(potreba);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> Edit(int id, MaterijalnaPotreba potreba)
    {
        String str = $"api/MaterijalnePotrebe/{id}";
        var json = JsonConvert.SerializeObject(potreba);
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

