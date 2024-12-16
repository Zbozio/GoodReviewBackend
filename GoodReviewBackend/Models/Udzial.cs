using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Udzial
{
    public int IdUdzialu { get; set; }

    public int? IdKsiazka { get; set; }

    public int? IdTypu { get; set; }

    public int? IdAutora { get; set; }

    public int? WartoscUdzialu { get; set; }

    public virtual Autor? IdAutoraNavigation { get; set; }

    public virtual Ksiazka? IdKsiazkaNavigation { get; set; }

    public virtual TypAutorstwa? IdTypuNavigation { get; set; }
}
