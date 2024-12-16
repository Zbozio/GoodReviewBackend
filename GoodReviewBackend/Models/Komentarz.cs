using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Komentarz
{
    public int IdOceny3 { get; set; }

    public int? IdRecenzji { get; set; }

    public int? IdUzytkownik { get; set; }

    public string? TrescKomentarza { get; set; }

    public virtual Recenzja? IdRecenzjiNavigation { get; set; }

    public virtual Uzytkownik? IdUzytkownikNavigation { get; set; }
}
