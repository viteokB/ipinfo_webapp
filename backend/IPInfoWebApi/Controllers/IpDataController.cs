using System.Net;
using Asp.Versioning;
using IPInfoWebApi.Clients.cs;
using IPInfoWebApi.Controllers.DTOs.ResponseDTOs;
using IPInfoWebApi.ResponseDTOs.AbuseIp;
using IPInfoWebApi.ResponseDTOs.FreeGeoIp;
using IPInfoWebApi.ResponseDTOs.IpInfo;
using Microsoft.AspNetCore.Mvc;

namespace IPInfoWebApi.Controllers;

[ApiController]
[Route("api/")]
[ApiVersion("1.0")]
public class IpDataController(IpDataService ipDataService) : ControllerBase
{
    private readonly IpDataService _ipDataService = ipDataService;

    #region Общие методы валидации
    
    private bool IsValidIp(string ip) => IPAddress.TryParse(ip, out _);

    private string? GetClientIp()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            ipAddress = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
        }

        return !string.IsNullOrEmpty(ipAddress) && IsValidIp(ipAddress) ? ipAddress : null;
    }
    
    #endregion

    #region Endpoint'ы для отдельных сервисов
    
    [HttpGet("ipinfo/{ip}")]
    [ProducesResponseType(typeof(IpInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetIpInfo(string ip)
    {
        if (!IsValidIp(ip))
            return UnprocessableEntity(new MyResponseMessage($"Invalid IP address format: {ip}"));

        var result = await _ipDataService.GetIpInfoAsync(ip);
        
        return result.IsSuccess 
            ? Ok(result.Data) 
            : BadRequest(new MyResponseMessage(result.ErrorText));
    }

    [HttpGet("freegeoip/{ip}")]
    [ProducesResponseType(typeof(FreeGeoIpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFreeGeoIp(string ip)
    {
        if (!IsValidIp(ip))
            return UnprocessableEntity(new MyResponseMessage($"Invalid IP address format: {ip}"));

        var result = await _ipDataService.GetFreeGeoIpAsync(ip);
        return result.IsSuccess 
            ? Ok(result.Data) 
            : BadRequest(new MyResponseMessage(result.ErrorText));
    }

    [HttpGet("abuseipdb/{ip}")]
    [ProducesResponseType(typeof(AbuseIpDbResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAbuseIpDbInfo(string ip, [FromQuery] int maxAgeInDays = 90)
    {
        if (!IsValidIp(ip))
            return UnprocessableEntity(new MyResponseMessage($"Invalid IP address format: {ip}"));

        var result = await _ipDataService.GetAbuseIpDbInfoAsync(ip, maxAgeInDays);
        return result.IsSuccess 
            ? Ok(result.Data) 
            : BadRequest(new MyResponseMessage(result.ErrorText));
    }
    
    #endregion

    #region Endpoint'ы для клиентского IP
    
    [HttpGet("ipinfo/my")]
    [ProducesResponseType(typeof(IpInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetMyIpInfo()
    {
        var ip = GetClientIp();
        return ip == null 
            ? UnprocessableEntity(new MyResponseMessage("Could not determine client IP")) 
            : await GetIpInfo(ip);
    }

    [HttpGet("freegeoip/my")]
    [ProducesResponseType(typeof(FreeGeoIpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetMyFreeGeoIp()
    {
        var ip = GetClientIp();
        return ip == null 
            ? UnprocessableEntity(new MyResponseMessage("Could not determine client IP")) 
            : await GetFreeGeoIp(ip);
    }

    [HttpGet("abuseipdb/my")]
    [ProducesResponseType(typeof(AbuseIpDbResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetMyAbuseIpDbInfo([FromQuery] int maxAgeInDays = 90)
    {
        var ip = GetClientIp();
        return ip == null 
            ? UnprocessableEntity(new MyResponseMessage("Could not determine client IP")) 
            : await GetAbuseIpDbInfo(ip, maxAgeInDays);
    }
    
    #endregion

    #region Комбинированный endpoint, возвращающий инфу от всех сервисов
    
    [HttpGet("full/{ip}")]
    [ProducesResponseType(typeof(IPDataResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFullIpData(string ip)
    {
        if (!IsValidIp(ip))
            return UnprocessableEntity(new MyResponseMessage($"Invalid IP address format: {ip}"));
        
        var ipInfo = await _ipDataService.GetIpInfoAsync(ip);
        var freeGeoIp = await _ipDataService.GetFreeGeoIpAsync(ip);
        var abuseIpDb = await _ipDataService.GetAbuseIpDbInfoAsync(ip, 240);

        //Можно придумать покрасивее, но пофиг+пофиг
        var errors = new List<string>(); 
        if (!ipInfo.IsSuccess)
            errors.Add($"IpInfo: {ipInfo.ErrorText}, HTTP StatusCode = {ipInfo.StatusCode}");
        if (!freeGeoIp.IsSuccess) 
            errors.Add($"FreeGeoIp: {freeGeoIp.ErrorText}, HTTP StatusCode = {ipInfo.StatusCode}");
        if (!abuseIpDb.IsSuccess) 
            errors.Add($"AbuseIpDb: {abuseIpDb.ErrorText}, HTTP StatusCode = {ipInfo.StatusCode}");

        return errors.Count > 0
            ? BadRequest(new MyResponseMessage("Failed to get complete IP data", errors))
            : Ok(new IPDataResponseDTO
            {
                IpInfoResponse = ipInfo.Data,
                FreeGeoIpResponse = freeGeoIp.Data,
                AbuseIpDbResponse = abuseIpDb.Data
            });
    }

    [HttpGet("full/my")]
    [ProducesResponseType(typeof(IPDataResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MyResponseMessage), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetMyFullIpData()
    {
        var ip = GetClientIp();
        return ip == null 
            ? UnprocessableEntity(new MyResponseMessage("Could not determine client IP")) 
            : await GetFullIpData(ip);
    }
    
    #endregion
    
    # region 
    
    [HttpOptions]
    // [HttpGet("ipinfo/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CheckApiStatus()
    {
        try
        {
            Response.Headers.Append("Allow", "GET, OPTIONS");
        
            return Ok();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "API unavailable");
        }
    }
    
    #endregion
}