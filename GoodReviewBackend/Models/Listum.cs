using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Listum
{
    public int IdListy { get; set; }

    public int? IdUzytkownik { get; set; }

    public string? NazwaListy { get; set; }

    public string? OpisListy { get; set; }

    public int? IloscElementow { get; set; }

    public bool? Prywatna { get; set; }

    public virtual Uzytkownik? IdUzytkownikNavigation { get; set; }

    public virtual ICollection<Ksiazka> IdKsiazkas { get; set; } = new List<Ksiazka>();
}
