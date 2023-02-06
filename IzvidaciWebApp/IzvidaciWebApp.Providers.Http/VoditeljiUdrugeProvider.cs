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

public class VoditeljiUdrugeProvider : IVoditeljiUdrugeProvider
{
    private readonly VoditeljUdrugeProviderOptions _options;
    private readonly HttpClient _httpClient;

    public VoditeljiUdrugeProvider(VoditeljUdrugeProviderOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClient = httpClientFactory.CreateClient("VoditeljiUdrugeOptions");
    }


    public async Task<Result<VoditeljiUdruge>> Get(int id)
    {
        var voditeljUdrugeDto = (await _httpClient.GetFromJsonAsync<VoditeljiUdrugeDto>($"/api/VoditeljiUdruge/{id}"));
        if (voditeljUdrugeDto is not null)
        {
            var voditeljUdruge = DtoMapping.ToDomain(voditeljUdrugeDto);
            return Results.OnSuccess<IzvidaciWebApp.Domain.Models.VoditeljiUdruge>(voditeljUdruge);
        }
        return Results.OnFailure<IzvidaciWebApp.Domain.Models.VoditeljiUdruge>("Voditelj udruge ne postoji");
    }

    public async Task<Result<IEnumerable<VoditeljiUdruge>>> GetAll()
    {
        var voditeljUdrugeDto = (await _httpClient.GetFromJsonAsync<IEnumerable<VoditeljiUdrugeDto>>($"/api/VoditeljiUdruge"));

        if (voditeljUdrugeDto is not null)
        {
            var voditeljUdruge = voditeljUdrugeDto.Select(r => DtoMapping.ToDomain(r));
            return Results.OnSuccess<IEnumerable<IzvidaciWebApp.Domain.Models.VoditeljiUdruge>>(voditeljUdruge);
        }
        return Results.OnFailure<IEnumerable<IzvidaciWebApp.Domain.Models.VoditeljiUdruge>>("Voditelji udruge ne postoje");
    }

    public async Task<Result> Delete(int id)
    {
        String str = "api/VoditeljiUdruge/" + id + "";
        var response = _httpClient.DeleteAsync(str).IsCompletedSuccessfully;
        if (!response)
        {
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno obrisano");
    }

    public async Task<Result> Create(VoditeljiUdruge voditeljUdruge)
    {
        String str = "api/VoditeljiUdruge/";
        var json = JsonConvert.SerializeObject(voditeljUdruge);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(str, data);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ne ide");
            return Results.OnFailure("Neuspjesno");
        }

        return Results.OnSuccess("Uspjesno");
    }

    public async Task<Result> Edit(int id, VoditeljiUdruge voditeljUdruge)
    {
        String str = "api/VoditeljiUdruge/" + id;
        var json = JsonConvert.SerializeObject(voditeljUdruge);
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
