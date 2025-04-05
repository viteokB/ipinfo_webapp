namespace IPInfoWebApi.Models.FreeGeoIp;

public class Location
{
    public int GeonamesId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Zip { get; set; }
    public Continent Continent { get; set; }
    public Country Country { get; set; }
    public City City { get; set; }
    public Region Region { get; set; }
}

// Континент
public class Continent
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string NameTranslated { get; set; }
    public int GeonamesId { get; set; }
    public string WikidataId { get; set; }
}

// Страна
public class Country
{
    public string Alpha2 { get; set; }
    public string Alpha3 { get; set; }
    public List<string> CallingCodes { get; set; }
    public List<Currency> Currencies { get; set; }
    public string Emoji { get; set; }
    public string Ioc { get; set; }
    public List<Language> Languages { get; set; }
    public string Name { get; set; }
    public string NameTranslated { get; set; }
    public List<string> Timezones { get; set; }
    public bool IsInEuropeanUnion { get; set; }
    public string Fips { get; set; }
    public int GeonamesId { get; set; }
    public string HascId { get; set; }
    public string WikidataId { get; set; }
}

// Валюта
public class Currency
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string SymbolNative { get; set; }
    public int DecimalDigits { get; set; }
    public int Rounding { get; set; }
    public string Code { get; set; }
    public string NamePlural { get; set; }
}

// Язык
public class Language
{
    public string Name { get; set; }
    public string NameNative { get; set; }
}

// Город
public class City
{
    public string Fips { get; set; }
    public string Alpha2 { get; set; }
    public int GeonamesId { get; set; }
    public string HascId { get; set; }
    public string WikidataId { get; set; }
    public string Name { get; set; }
    public string NameTranslated { get; set; }
}

// Регион (штат/область)
public class Region
{
    public string Fips { get; set; }
    public string Alpha2 { get; set; }
    public int GeonamesId { get; set; }
    public string HascId { get; set; }
    public string WikidataId { get; set; }
    public string Name { get; set; }
    public string NameTranslated { get; set; }
}