using System.Net.Http.Json;
using System.Text;
using BaseLibrary;
using IzvidaciWebApp.Providers.Http.Dtos;
using IzvidaciWebApp.Providers.Http.Options;
using Newtonsoft.Json;
using PotrosniResurs = IzvidaciWebApp.Domain.Models.PotrosniResurs;
using Resurs = IzvidaciWebApp.Domain.Models.Resurs;
using TrajniResurs = IzvidaciWebApp.Domain.Models.TrajniResurs;

namespace IzvidaciWebApp.Providers.Http;

public class ResursiProvider : IResursiProvider
{
    
    private readonly UdrugaProviderOptions _options;
    private readonly HttpClient _httpClient;

    public ResursiProvider(UdrugaProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("UdrugeOptions");
    }

    public async Task<Result<PotrosniResurs>> GetPotrosni(int id)
    {
        var resursDto = await _httpClient.GetFromJsonAsync<PotrosniResursDto>($"/api/Resurs/Potrosni/"+id);
        if (resursDto is not null)
        {
            var resurs = DtoMapping.ToDomain(resursDto);
            return Results.OnSuccess(resurs);
        }
        return Results.OnFailure<PotrosniResurs>("Resurs ne postoji");
    }
    
    public async Task<Result<TrajniResurs>> GetTrajni(int id)
    {
        var resursDto = await _httpClient.GetFromJsonAsync<TrajniResursDto>($"/api/Resurs/Trajni/"+id);
        if (resursDto is not null)
        {
            var resurs = DtoMapping.ToDomain(resursDto);
            return Results.OnSuccess(resurs);
        }
        return Results.OnFailure<TrajniResurs>("Resurs ne postoji");
    }

    public async Task<Result<IEnumerable<Resurs>>> GetAll()
    {
        var resursDto = await _httpClient.GetFromJsonAsync<IEnumerable<ResursDto>>($"/api/Resurs");

        if (resursDto is not null)
        {
            var resurs = resursDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess(resurs);
        }
        return Results.OnFailure<IEnumerable<Resurs>>("Ne postoji niti jedan resurs");
    }

    public async Task<Result<IEnumerable<PotrosniResurs>>> GetAllPotrosni()
    {
        var resursDto = await _httpClient.GetFromJsonAsync<IEnumerable<PotrosniResursDto>>($"/api/Resurs/PotrosniResursi");

        if (resursDto is not null)
        {
            var resurs = resursDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess(resurs);
        }
        return Results.OnFailure<IEnumerable<PotrosniResurs>>("Ne postoji niti jedan resurs");
    }

    public async Task<Result<IEnumerable<TrajniResurs>>> GetAllTrajni()
    {
        var resursDto = await _httpClient.GetFromJsonAsync<IEnumerable<TrajniResursDto>>($"/api/Resurs/TrajniResursi");

        if (resursDto is not null)
        {
            var resurs = resursDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess(resurs);
        }
        return Results.OnFailure<IEnumerable<TrajniResurs>>("Ne postoji niti jedan resurs");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/Resurs/" + id;
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> CreatePotrosni(PotrosniResurs resurs)
    {
        var json = JsonConvert.SerializeObject(resurs);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Resurs/PotrosniResursi", data);
        if (!response.IsSuccessStatusCode)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }
    
    public async Task<Result> CreateTrajni(TrajniResurs resurs)
    {
        var json = JsonConvert.SerializeObject(resurs);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Resurs/TrajniResursi", data);
        if (!response.IsSuccessStatusCode)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }
    
    public async Task<Result> EditTrajni(int id, TrajniResurs resurs)
    {
        String str = "api/Resurs/TrajniResurs/" + id;
        var json = JsonConvert.SerializeObject(resurs);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PutAsync(str, data);
        if (!response.IsCompleted)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> EditPotrosni(int id, PotrosniResurs resurs)
    {
        String str = "api/Resurs/PotrosniResurs/" + id;
        var json = JsonConvert.SerializeObject(resurs);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _httpClient.PutAsync(str, data);
        if (!response.IsCompleted)
        {
            return Results.OnFailure("Neuspjesno");
        }
        return Results.OnSuccess("Uspjesno");
    }
}