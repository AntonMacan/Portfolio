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

public class UdrugeProvider : IUdrugeProvider
{
    private readonly UdrugaProviderOptions _options;
    private readonly HttpClient _httpClient;

    public UdrugeProvider(UdrugaProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("UdrugeOptions");
    }


    public async Task<Result<Udruge>> Get(int id)
    {
        var udrugaDto = (await _httpClient.GetFromJsonAsync<UdrugeDto>($"/api/Udruge/{id}"));
        if (udrugaDto is not null)
        {
            var udruga = DtoMapping.ToDomain(udrugaDto);
            return Results.OnSuccess<IzvidaciWebApp.Domain.Models.Udruge>(udruga);
        }
        return Results.OnFailure<IzvidaciWebApp.Domain.Models.Udruge>("Udruga ne postoji");
    }

    public async Task<Result<IEnumerable<Udruge>>> GetAll()
    {
        var udrugaDto = (await _httpClient.GetFromJsonAsync<IEnumerable<UdrugeDto>>($"/api/Udruge"));

        if (udrugaDto is not null)
        {
            var udruga = udrugaDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess<IEnumerable<IzvidaciWebApp.Domain.Models.Udruge>>(udruga);
        }
        return Results.OnFailure<IEnumerable<IzvidaciWebApp.Domain.Models.Udruge>>("Udruge ne postoji");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/Udruge/" + id + "";
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(Udruge udruga)
    {
        String str = "api/Udruge/";
        var json = JsonConvert.SerializeObject(udruga);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> Edit(int id, Udruge udruga)
    {
        String str = "api/Udruge/" + id;
        var json = JsonConvert.SerializeObject(udruga);
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
