using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Autor
{
    public int IdAutora { get; set; }

    public string? ImieAutora { get; set; }

    public string? NazwiskoAutora { get; set; }

    public int? Wiek { get; set; }

    public DateTime? DataUrodzenia { get; set; }

    public string? Opis { get; set; }

    public DateTime? DataSmierci { get; set; }

    public string? Pseudonim { get; set; }

    public virtual ICollection<Udzial> Udzials { get; set; } = new List<Udzial>();
}
