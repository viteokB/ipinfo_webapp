namespace IPInfoWebApi.Models.FreeGeoIp;

public class Connection
{
    public int Asn { get; set; }
    
    public string Organization { get; set; }
    
    public string Isp { get; set; }
    
    public string Range { get; set; }
}