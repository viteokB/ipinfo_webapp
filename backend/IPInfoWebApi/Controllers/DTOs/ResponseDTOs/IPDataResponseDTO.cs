using IPInfoWebApi.ResponseDTOs.AbuseIp;
using IPInfoWebApi.ResponseDTOs.FreeGeoIp;
using IPInfoWebApi.ResponseDTOs.IpInfo;

namespace IPInfoWebApi.Controllers.DTOs.ResponseDTOs;

public class IPDataResponseDTO
{
    public IpInfoResponse IpInfoResponse { get; set; }
    
    public FreeGeoIpResponse FreeGeoIpResponse { get; set; }
    
    public AbuseIpDbResponse AbuseIpDbResponse { get; set; }
}