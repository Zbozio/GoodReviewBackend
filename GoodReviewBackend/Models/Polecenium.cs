using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Polecenium
{
    public int IdPolecenia { get; set; }

    public int IdZnajomosci { get; set; }

    public int IdKsiazka { get; set; }

    public string? TrescPolecenia { get; set; }

    public DateTime? DataPolecenia { get; set; }
    [JsonIgnore]
    [NotMapped]

    public virtual Ksiazka IdKsiazkaNavigation { get; set; } = null!;
    [JsonIgnore]
    [NotMapped]
    public virtual Znajomi IdZnajomosciNavigation { get; set; } = null!;
}
