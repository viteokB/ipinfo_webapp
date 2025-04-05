namespace IPInfoWebApi.Models;

/// <summary>
/// Хранит токены для внешних API.
/// </summary>
/// <param name="IpInfoToken">Токен для ipinfo.io</param>
/// <param name="FreeGeoIpToken">Токен для FreeGeoIP.app</param>
/// <param name="AbuseIpDbToken">Токен для AbuseIPDB</param>
public class TokensPool
{
    public TokensPool() { } // Добавляем пустой конструктор

    public TokensPool(string ipInfoToken, string freeGeoIpToken, string abuseIpDbToken)
    {
        IpInfoToken = ipInfoToken;
        FreeGeoIpToken = freeGeoIpToken;
        AbuseIpDbToken = abuseIpDbToken;
    }

    public string IpInfoToken { get; set; }
    public string FreeGeoIpToken { get; set; }
    public string AbuseIpDbToken { get; set; }
}