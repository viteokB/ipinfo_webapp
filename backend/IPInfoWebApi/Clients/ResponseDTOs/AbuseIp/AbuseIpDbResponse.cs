namespace IPInfoWebApi.ResponseDTOs.AbuseIp;

public class AbuseIpDbResponse
{
    public AbuseIpDbData? Data { get; set; }
}

public class AbuseIpDbData
{
    public string? IpAddress { get; set; }
    public bool IsPublic { get; set; }
    public int IpVersion { get; set; }
    public bool? IsWhitelisted { get; set; }  // Nullable, так как в JSON может быть null
    public int AbuseConfidenceScore { get; set; }
    public string? CountryCode { get; set; }
    public string? UsageType { get; set; }
    public string? Isp { get; set; }
    public string? Domain { get; set; }
    public List<string>? Hostnames { get; set; }
    public bool IsTor { get; set; }
    public int TotalReports { get; set; }
    public int NumDistinctUsers { get; set; }
    public DateTimeOffset? LastReportedAt { get; set; }  // Nullable
}