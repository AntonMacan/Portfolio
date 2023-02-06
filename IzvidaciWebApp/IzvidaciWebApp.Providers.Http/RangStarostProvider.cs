using System.Net;
using System.Net.Http.Json;
using System.Text;
using BaseLibrary;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;

namespace IzvidaciWebApp.Providers.Http;

public class RangStarostProvider : IRangStarostProvider
{
    private readonly RangStarostProviderOptions _options;
    private readonly HttpClient _httpClient;
    
    public RangStarostProvider(RangStarostProviderOptions options,IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("RangStarostOptions");
    }

    public async Task<Result<RangStarost>> Get(int id)
    {
        var rangDto = (await _httpClient.GetFromJsonAsync<RangStarostDTO>($"/api/RangStarost/{id}"));
        if (rangDto is not null)
        {
            var rang = DtoMapping.ToDomain(rangDto);
            return Results.OnSuccess(rang);
        }
        return Results.OnFailure<RangStarost>("Rang ne postoji");
    }

    public async Task<Result<IEnumerable<RangStarost>>> GetAll()
    {  
        var rangDto = (await _httpClient.GetFromJsonAsync<IEnumerable<RangStarostDTO>>($"/api/RangStarost"));

        if (rangDto is not null)
        {
            var rang = rangDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess(rang);
        }
        return Results.OnFailure<IEnumerable<RangStarost>>("Rangovi ne postoje");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/RangStarost/"+id + "";
        var response = _httpClient.DeleteAsync(str);
        if (!response.Result.IsSuccessStatusCode)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(RangStarost rangStarost)
    {
        String str = "api/RangStarost/";
        var json = JsonConvert.SerializeObject(rangStarost);
        var data = new StringContent(json,Encoding.UTF8,"application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Results.OnFailure("Greska pri dodavanju ranga po starosti!");
        }

        return Results.OnSuccess("Uspjesno dodano");
    }

    public async Task<Result> Edit(int id,RangStarost rangStarost)
    {
        String str = "api/RangStarost/"+id;
        var json = JsonConvert.SerializeObject(rangStarost);
        var data = new StringContent(json,Encoding.UTF8,"application/json");
        var response = await _httpClient.PutAsync(str,data);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Results.OnFailure("Neuspjesno uredivanje ranga po starosti");
        }
        return Results.OnSuccess("Uspjesno uredivanje ranga po starosti");
    }
}