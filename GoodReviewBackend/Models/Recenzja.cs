using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Recenzja
{
    public int IdRecenzji { get; set; }

    public int IdKsiazka { get; set; }

    public int IdUzytkownik { get; set; }

    public string? TrescRecenzji { get; set; }

    public DateTime? DataRecenzji { get; set; }

    public int? PolubieniaRecenzji { get; set; }

    public virtual Ksiazka IdKsiazkaNavigation { get; set; } = null!;

    public virtual Uzytkownik IdUzytkownikNavigation { get; set; } = null!;

    public virtual ICollection<Komentarz> Komentarzs { get; set; } = new List<Komentarz>();
}
