using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Gatunek
{
    public int IdGatunku { get; set; }

    public string? NazwaGatunku { get; set; }
    [JsonIgnore]
    public virtual ICollection<Ksiazka> IdKsiazkas { get; set; } = new List<Ksiazka>();
    [JsonIgnore]

    public virtual ICollection<Uzytkownik> IdUzytkowniks { get; set; } = new List<Uzytkownik>();




}
