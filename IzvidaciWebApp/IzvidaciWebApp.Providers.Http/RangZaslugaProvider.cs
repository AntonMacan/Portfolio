using System.Collections;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Models;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;

namespace IzvidaciWebApp.Providers.Http;

public class RangZaslugaProvider : IRangZaslugaProvider
{
    private readonly RangZaslugaProviderOptions _options;
    private readonly HttpClient _httpClient;
    
    public RangZaslugaProvider(RangZaslugaProviderOptions options,IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("RangZaslugaOptions");
    }

    public async Task<Result<RangZasluga>> Get(int id)
    {
        var rangDto = (await _httpClient.GetFromJsonAsync<RangZaslugaDto>($"/api/RangZasluga/{id}"));
        if (rangDto is not null)
        {
            var rang = DtoMapping.ToDomain(rangDto);
            return Results.OnSuccess(rang);
        }
        return Results.OnFailure<RangZasluga>("Rang ne postoji");
    }

    public async Task<Result<IEnumerable<RangZasluga>>> GetAll()
    {  
        var rangDto = (await _httpClient.GetFromJsonAsync<IEnumerable<RangZaslugaDto>>($"/api/RangZasluga"));

        if (rangDto is not null)
        {
            var rang = rangDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess(rang);
        }
        return Results.OnFailure<IEnumerable<RangZasluga>>("Rangovi ne postoje");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/RangZasluga/"+id + "";
        var response = _httpClient.DeleteAsync(str);
        if (!response.Result.IsSuccessStatusCode)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(RangZasluga rangZasluga)
    {
        String str = "api/RangZasluga/";
        var json = JsonConvert.SerializeObject(rangZasluga);
        var data = new StringContent(json,Encoding.UTF8,"application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Results.OnFailure("Greska pri dodavanju ranga po zasluzi!");
        }

        return Results.OnSuccess("Uspjesno dodano!");
    }

    public async Task<Result> Edit(int id,RangZasluga rangZasluga)
    {
        String str = "api/RangZasluga/"+id;
        var json = JsonConvert.SerializeObject(rangZasluga);
        var data = new StringContent(json,Encoding.UTF8,"application/json");
        var response = await _httpClient.PutAsync(str,data);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Results.OnFailure("Neuspjesno uredivanje ranga po zasluzi");
        }
        return Results.OnSuccess("Uspjesno uredivanje ranga po zasluzi");
    }
}