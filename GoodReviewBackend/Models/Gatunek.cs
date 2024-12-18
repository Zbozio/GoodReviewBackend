using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Gatunek
{
    public int IdGatunku { get; set; }

    public string? NazwaGatunku { get; set; }

    public virtual ICollection<Ksiazka> IdKsiazkas { get; set; } = new List<Ksiazka>();

    public virtual ICollection<Uzytkownik> IdUzytkowniks { get; set; } = new List<Uzytkownik>();




}
