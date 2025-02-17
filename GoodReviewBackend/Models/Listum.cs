using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Listum
{
    public int IdListy { get; set; }

    public int? IdUzytkownik { get; set; }

    public string? NazwaListy { get; set; }

    public string? OpisListy { get; set; }

    public int? IloscElementow { get; set; }

    public bool? Prywatna { get; set; }
    [JsonIgnore]
    public virtual Uzytkownik? IdUzytkownikNavigation { get; set; }
    [JsonIgnore]

    public virtual ICollection<Ksiazka> IdKsiazkas { get; set; } = new List<Ksiazka>();
}
