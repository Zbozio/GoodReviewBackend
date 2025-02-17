using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Udzial
{
    public int IdUdzialu { get; set; }

    public int? IdKsiazka { get; set; }

    public int? IdTypu { get; set; }

    public int? IdAutora { get; set; }

    public int? WartoscUdzialu { get; set; }
    [JsonIgnore]
    public virtual Autor? IdAutoraNavigation { get; set; }
    [JsonIgnore]
    public virtual Ksiazka? IdKsiazkaNavigation { get; set; }
    [JsonIgnore]
    public virtual TypAutorstwa? IdTypuNavigation { get; set; }
}
