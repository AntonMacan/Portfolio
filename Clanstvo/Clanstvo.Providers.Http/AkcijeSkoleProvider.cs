using System.Net.Http.Json;
using BaseLibrary;
using Clanstvo.Domain.Models;
using Clanstvo.Providers.Http.DTOs;
using Clanstvo.Providers.Http.Options;

namespace Clanstvo.Providers.Http;

public class AkcijeSkoleProvider : IAkcijeSkoleProvider
{
    //private readonly HttpClient _httpClient;
    private readonly AkcijeSkoleProviderOptions _options;

    public AkcijeSkoleProvider(AkcijeSkoleProviderOptions akcijeSkoleProvidersOptions)
    {
        //_httpClient = httpClientFactory.CreateClient("AkcijeSkole");
        _options = akcijeSkoleProvidersOptions;
    }

    
    public async Task<Result<IEnumerable<Akcija>>> GetAkcijeClana(int id)
    {
        //var akcijaResult = _httpClient.GetFromJsonAsync<IEnumerable<AkcijaSudionik>>($"/polaznici/{id}");
        IEnumerable<AkcijaSudionik>? akcijeResult;

        using (var httpClientHandler = new HttpClientHandler())
        {
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.BaseAddress = new Uri(_options.BaseUrl);
                akcijeResult = await httpClient.GetFromJsonAsync<IEnumerable<AkcijaSudionik>>($"/polaznici/{id}");
            }
        }

        if (akcijeResult != null) // skoleResult!.Any() => onda baci 404 i poruku a ne praznu listu
        {
            var akcije = akcijeResult!.Select(a => AkcijaSudionik.DtoMapping.ToDomain(a));

            return Results.OnSuccess(akcije);
        }

        return Results.OnFailure<IEnumerable<Akcija>>("Clan nema zabiljezenih akcija");
    }

    public async Task<Result<IEnumerable<Skola>>> GetSkoleClana(int id)
    {
        //var akcijaResult = _httpClient.GetFromJsonAsync<IEnumerable<AkcijaSudionik>>($"/polaznici/{id}");
        IEnumerable<SkolaSudionik>? skoleResult;

        using (var httpClientHandler = new HttpClientHandler())
        {
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.BaseAddress = new Uri(_options.BaseUrl);
                skoleResult = await httpClient.GetFromJsonAsync<IEnumerable<SkolaSudionik>>($"/polaznik/{id}");
            }
        }

        if (skoleResult != null) // skoleResult!.Any() => onda baci 404 i poruku a ne praznu listu
        {
            var akcije = skoleResult!.Select(a => SkolaSudionik.DtoMapping.ToDomain(a));

            return Results.OnSuccess(akcije);
        }

        return Results.OnFailure<IEnumerable<Skola>>("Clan nema zabiljezenih skola");
    }

}