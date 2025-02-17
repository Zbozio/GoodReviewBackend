using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Status
{
    public int IdStatusu { get; set; }

    public int? IdKsiazka { get; set; }

    public int? IdUzytkownik { get; set; }

    public int? IdStatusNazwa { get; set; }

    public string? KomentarzStatusu { get; set; }

    public DateTime? DataStatusu { get; set; }
    [JsonIgnore]

    public virtual Ksiazka? IdKsiazkaNavigation { get; set; }

    public virtual StatusNazwa? IdStatusNazwaNavigation { get; set; }

    public virtual Uzytkownik? IdUzytkownikNavigation { get; set; }
}
