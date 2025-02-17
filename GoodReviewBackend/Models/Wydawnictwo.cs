using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Wydawnictwo
{
    public int IdWydawnictwa { get; set; }

    public string? Nazwa { get; set; }

    public string? AdresSiedziby { get; set; }

    public string? StronaInternetowa { get; set; }
    [JsonIgnore]

    public virtual ICollection<Ksiazka> Ksiazkas { get; set; } = new List<Ksiazka>();
}
