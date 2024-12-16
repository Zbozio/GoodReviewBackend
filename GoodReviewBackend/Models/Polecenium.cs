using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Polecenium
{
    public int IdPolecenia { get; set; }

    public int IdZnajomosci { get; set; }

    public int IdKsiazka { get; set; }

    public string? TrescPolecenia { get; set; }

    public DateTime? DataPolecenia { get; set; }

    public virtual Ksiazka IdKsiazkaNavigation { get; set; } = null!;

    public virtual Znajomi IdZnajomosciNavigation { get; set; } = null!;
}
