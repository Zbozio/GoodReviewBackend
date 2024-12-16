using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class StatusNazwa
{
    public int IdStatusNazwa { get; set; }

    public string? NazwaStatusu { get; set; }

    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();
}
