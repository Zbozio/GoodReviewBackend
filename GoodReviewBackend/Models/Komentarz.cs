using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Komentarz
{
    public int IdOceny3 { get; set; }

    public int? IdRecenzji { get; set; }

    public int? IdUzytkownik { get; set; }

    public string? TrescKomentarza { get; set; }
    [JsonIgnore]

    public virtual Recenzja? IdRecenzjiNavigation { get; set; }
    [JsonIgnore]
    public virtual Uzytkownik? IdUzytkownikNavigation { get; set; }
}
