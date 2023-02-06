using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

public class CvrstiObjektZaObitavanjeProvider : ICvrstiObjektZaObitavanjeProvider
{
    private readonly CvrstiObjektZaObitavanjeProviderOptions _options;
    private readonly HttpClient _httpClient;

    public CvrstiObjektZaObitavanjeProvider(CvrstiObjektZaObitavanjeProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("CvrstiObjektZaObitavanjeOptions");
    }

    //fali
    public async Task<Result<CvrstiObjektZaObitavanje>> Get(int id)
    {
        var objektDTO = (await _httpClient.GetFromJsonAsync<CvrstiObjektZaObitavanjeDto>($"/api/TerenskaLokacija/CvrstiObjektiZaObitavanje/{id}"));

        if(objektDTO is not null)
        {
            var objekt = DtoMapping.ToDomain(objektDTO);

            return Results.OnSuccess<CvrstiObjektZaObitavanje>(objekt);
        }

        return Results.OnFailure<CvrstiObjektZaObitavanje>("Cvrsti objekt za obitavanje ne postoji");
    }

    public async Task<Result<IEnumerable<CvrstiObjektZaObitavanje>>> GetAll()
    {
        var objektDTO = (await _httpClient.GetFromJsonAsync<IEnumerable<CvrstiObjektZaObitavanjeDto>>("/api/TerenskaLokacija/CvrstiObjektiZaObitavanje"));

        if(objektDTO is not null)
        {
            var objekt = objektDTO.Select(o => DtoMapping.ToDomain(o));

            return Results.OnSuccess<IEnumerable<CvrstiObjektZaObitavanje>>(objekt);
        }

        return Results.OnFailure<IEnumerable<CvrstiObjektZaObitavanje>>("Cvrsti objekti za obitavanje ne postoje");
    }

    //fali
    public async Task<Result> Delete(int id)
    {
        string str = $"/api/TerenskaLokacija/CvrstiObjektiZaObitavanje/{id}";

        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;

        if (!response)
        {
            return Results.OnFailure("Neuspješno brisanje");
        }

        return Results.OnSuccess("Uspješno obrisano");
    }

    public async Task<Result> Create(CvrstiObjektZaObitavanje objekt)
    {
        string str = "/api/TerenskaLokacija/CvrstiObjektiZaObitavanje";
        
        var json = JsonConvert.SerializeObject(objekt);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);

        if (!response.IsSuccessStatusCode)
        {
            return Results.OnFailure("Neuspješno dodavanje");
        }

        return Results.OnSuccess("Uspješno dodano");
    }

    public async Task<Result> Edit(int id, CvrstiObjektZaObitavanje objekt)
    {
        string str = $"/api/TerenskaLokacija/CvrstiObjektiZaObitavanje/{id}";

        var json = JsonConvert.SerializeObject(objekt);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PutAsync(str, data);

        if (!response.IsCompleted)
        {
            return Results.OnFailure("Neuspješno ažuriranje");
        }

        return Results.OnSuccess("Uspješno ažurirano");
    }
}