using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class TypAutorstwa
{
    public int IdTypu { get; set; }

    public string? NazwaTypu { get; set; }

    public virtual ICollection<Udzial> Udzials { get; set; } = new List<Udzial>();
}
