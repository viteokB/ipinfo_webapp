namespace IPInfoWebApi.ResponseDTOs.FreeGeoIp;

public class Timezone
{
    // Идентификатор временной зоны (например, "America/Los_Angeles")
    public string Id { get; set; }
    
    // Текущее время в зоне (в формате ISO 8601)
    public DateTimeOffset CurrentTime { get; set; }
    
    // Код часового пояса (например, "PDT")
    public string Code { get; set; }
    
    // Флаг летнего времени
    public bool IsDaylightSaving { get; set; }
    
    // Смещение от GMT в секундах (например, -25200 для PDT)
    public int GmtOffset { get; set; }
    
    // Дополнительно: можно добавить вычисляемое свойство для удобства
    public TimeSpan GmtOffsetTimeSpan => TimeSpan.FromSeconds(GmtOffset);
}