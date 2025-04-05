namespace IPInfoWebApi.Models.IpInfo;

public class IpInfoResponse
{
    public string Ip { get; set; }
    
    public string ASN { get; set; }
    
    public string As_Name { get; set; }
    
    public string As_Domain { get; set; }
    
    public string CountryCode { get; set; }
    
    public string ContinentCode { get; set; }
    
    public string Continent { get; set; }
}