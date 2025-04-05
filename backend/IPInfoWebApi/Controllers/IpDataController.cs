using Asp.Versioning;
using IPInfoWebApi.Clients.cs;
using IPInfoWebApi.Models.AbuseIp;
using Microsoft.AspNetCore.Mvc;

namespace IPInfoWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class IpDataController(IpDataService ipDataService) : ControllerBase
{
    private readonly IpDataService _ipDataService = ipDataService;

    [HttpGet("{ip}")]
    public async Task<IActionResult> GetIpData(string ip)
    {
        var ipInfo = await _ipDataService.GetIpInfoAsync(ip);
        var freeGeoIp = await _ipDataService.GetFreeGeoIpAsync(ip);
        var abuseIpDb = await _ipDataService.GetAbuseIpDbInfoAsync(ip, 180);

        if (!ipInfo.IsSuccess || !freeGeoIp.IsSuccess || !abuseIpDb.IsSuccess)
        {
            return BadRequest();
        }

        var result = new
        {
            IpInfo = ipInfo.Data,
            FreeGeoIp = freeGeoIp.Data,
            AbuseIpDb = abuseIpDb.Data,
        };

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var ipAddress = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");
        ipAddress ??= HttpContext.Connection.RemoteIpAddress.ToString();

        return await GetIpData(ipAddress);
    }
}