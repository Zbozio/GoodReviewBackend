using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

[Keyless]
public class GatunkowoscDto
{

    
    public int IdKsiazka { get; set; }
    
    public int IdGatunku { get; set; }

    [JsonIgnore]
    public string Tytul { get; set; }
    [JsonIgnore]
    public string NazwaGatunku { get; set; }
}
