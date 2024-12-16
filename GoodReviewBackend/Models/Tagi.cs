using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Tagi
{
    public int IdOceny5 { get; set; }

    public string? NazwaTagu { get; set; }

    public virtual ICollection<Ksiazka> IdKsiazkas { get; set; } = new List<Ksiazka>();
}
