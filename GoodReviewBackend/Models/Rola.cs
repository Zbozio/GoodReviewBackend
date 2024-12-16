using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Rola
{
    public int IdOceny2 { get; set; }

    public string? NazwaRoli { get; set; }

    public virtual ICollection<Uzytkownik> Uzytkowniks { get; set; } = new List<Uzytkownik>();
}
