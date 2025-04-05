
using System.Net;
using System.Net.Http.Headers;
using IPInfoWebApi.Helpers;
using IPInfoWebApi.Models;
using IPInfoWebApi.Models.AbuseIp;
using IPInfoWebApi.Models.FreeGeoIp;
using IPInfoWebApi.Models.IpInfo;
using Microsoft.Extensions.Options;

namespace IPInfoWebApi.Clients.cs;

public class IpDataService
{
    private readonly HttpClient _httpClient = new();

    private readonly TokensPool _tokensPool;

    public IpDataService(IOptions<TokensPool> tokensPool)
    {
        _tokensPool = tokensPool.Value;
    }

    public async Task<OperationResult<IpInfoResponse>> GetIpInfoAsync(string ip)
    {
        var response = await _httpClient.GetAsync($"https://api.ipinfo.io/lite/{ip}?token={_tokensPool.IpInfoToken}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<IpInfoResponse>();
            
            return OperationResult<IpInfoResponse>.Success(content);
        }
        
        return OperationResult<IpInfoResponse>.Error("Ошибка запроса", (int)response.StatusCode);
    }

    public async Task<OperationResult<FreeGeoIpResponse>> GetFreeGeoIpAsync(string ip)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.ipbase.com/v2/info?ip={ip}");
        requestMessage.Headers.Add("apikey", _tokensPool.FreeGeoIpToken);
        
        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<FreeGeoIpResponse>();
            
            return OperationResult<FreeGeoIpResponse>.Success(content);
        }
            
        return OperationResult<FreeGeoIpResponse>.Error("Ошибка запроса", (int)response.StatusCode);
    }

    public async Task<OperationResult<AbuseIpDbResponse>> GetAbuseIpDbInfoAsync(string ipAddress, int maxAgeInDays, bool verbose = false)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.abuseipdb.com/api/v2/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Key", _tokensPool.AbuseIpDbToken);

        var queryParams = new Dictionary<string, string>
        {
            ["ipAddress"] = ipAddress,
            ["maxAgeInDays"] = maxAgeInDays.ToString(),
            ["verbose"] = verbose ? "" : null // параметр без значения
        };
        
        var validParams = queryParams
            .Where(p => p.Value != null)
            .ToDictionary(p => p.Key, p => p.Value);

        var queryString = string.Join("&", validParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
        var requestUri = $"check?{queryString}";

        var response = await client.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<AbuseIpDbResponse>();

            return OperationResult<AbuseIpDbResponse>.Success(content);
        }

        return OperationResult<AbuseIpDbResponse>.Error("Ошибка запроса к Abuse", (int)response.StatusCode);
    }
}