using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models
{
    public partial class Gatunkowosc
    {
        public int IdKsiazka { get; set; }  // Klucz obcy do książki
        public int IdGatunek { get; set; }  // Klucz obcy do gatunku

        // Nawigacyjne właściwości
        public virtual Ksiazka? Ksiazka { get; set; }
        public virtual Gatunek? Gatunek { get; set; }
    }
}
