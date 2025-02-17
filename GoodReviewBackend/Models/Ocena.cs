using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class Ocena
{
    public int IdOceny { get; set; }

    public int? IdKsiazka { get; set; }

    public int? IdUzytkownik { get; set; }

    public DateTime? DataOceny { get; set; }

    public int? WartoscOceny { get; set; }
    [JsonIgnore]
    public virtual Ksiazka? IdKsiazkaNavigation { get; set; }
    [JsonIgnore]
    public virtual Uzytkownik? IdUzytkownikNavigation { get; set; }
}
