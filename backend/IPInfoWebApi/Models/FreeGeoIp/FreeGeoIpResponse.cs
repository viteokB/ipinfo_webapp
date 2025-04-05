namespace IPInfoWebApi.Models.FreeGeoIp;

public class FreeGeoIpResponse
{
    public Data Data { get; set; }
}

public class Data
{
    public string Ip { get; set; }
    
    public string Hostname { get; set; }
    
    public string Type { get; set; }
    
    public RangeType RangeType { get; set; }
    
    public Connection Connection { get; set; }
    
    public Location Location { get; set; }
    
    public Timezone Timezone { get; set; }
}

public class RangeType
{
    public string Type { get; set; }
    
    public string Description { get; set; }
}