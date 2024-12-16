using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Znajomi
{
    public int IdZnajomosci { get; set; }

    public int IdUzytkownik { get; set; }

    public int UzyIdUzytkownik { get; set; }

    public DateTime? DataZnajomosci { get; set; }

    public string? StatusZnajomosci { get; set; }

    public virtual Uzytkownik IdUzytkownikNavigation { get; set; } = null!;

    public virtual ICollection<Polecenium> Polecenia { get; set; } = new List<Polecenium>();

    public virtual Uzytkownik UzyIdUzytkownikNavigation { get; set; } = null!;
}
