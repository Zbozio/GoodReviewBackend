using System.Text.Json.Serialization;

public class UlubioneGatunkiDto
{
    [JsonIgnore]
    public int IdUzytkownik { get; set; }
    [JsonIgnore]
    public List<int> GatunkiIds { get; set; }
}
