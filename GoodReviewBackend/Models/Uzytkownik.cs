using GoodReviewBackend.Models;

public partial class Uzytkownik
{
    public int IdUzytkownik { get; set; }

    public int? IdOceny2 { get; set; }

    public string? Imie { get; set; }

    public string? Nazwisko { get; set; }

    public string? EMail { get; set; }

    public string? Haslo { get; set; }

    public DateTime? DataRejestracji { get; set; }

    public int? IloscOcen { get; set; }

    public int? IloscRecenzji { get; set; }

    public DateTime? OstaniaAktywnosc { get; set; }

    public string? Zdjecie { get; set; }

    public int? Znajomi { get; set; }

    public string? Opis { get; set; }

    // Nowa kolumna DataUrodzenia
    public DateTime? DataUrodzenia { get; set; }  // Nullable, jeśli nie ma obowiązku podawania daty urodzenia

    public virtual Rola? IdOceny2Navigation { get; set; }

    public virtual ICollection<Komentarz> Komentarzs { get; set; } = new List<Komentarz>();

    public virtual ICollection<Listum> Lista { get; set; } = new List<Listum>();

    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();

    public virtual ICollection<Recenzja> Recenzjas { get; set; } = new List<Recenzja>();

    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();

    public virtual ICollection<Znajomi> ZnajomiIdUzytkownikNavigations { get; set; } = new List<Znajomi>();

    public virtual ICollection<Znajomi> ZnajomiUzyIdUzytkownikNavigations { get; set; } = new List<Znajomi>();

    public virtual ICollection<Gatunek> IdGatunkus { get; set; } = new List<Gatunek>();

    public virtual ICollection<Ksiazka> IdKsiazkas { get; set; } = new List<Ksiazka>();
}
