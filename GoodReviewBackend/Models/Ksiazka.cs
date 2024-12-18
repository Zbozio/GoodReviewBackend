using System;
using System.Collections.Generic;

namespace GoodReviewBackend.Models;

public partial class Ksiazka
{
    public int IdKsiazka { get; set; }

    public int? IdWydawnictwa { get; set; }

    public string? Tytul { get; set; }

    public string? Opis { get; set; }

    public DateTime? RokWydania { get; set; }

    public int? LiczbaOcen { get; set; }

    public int? IloscStron { get; set; }

    public string? Okladka { get; set; }

    public string? Isbn { get; set; }

    public virtual Wydawnictwo? IdWydawnictwaNavigation { get; set; }

    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();

    public virtual ICollection<Polecenium> Polecenia { get; set; } = new List<Polecenium>();

    public virtual ICollection<Recenzja> Recenzjas { get; set; } = new List<Recenzja>();

    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();

    public virtual ICollection<Udzial> Udzials { get; set; } = new List<Udzial>();

    public virtual ICollection<Gatunek> IdGatunkus { get; set; } = new List<Gatunek>();

    public virtual ICollection<Listum> IdListies { get; set; } = new List<Listum>();

    public virtual ICollection<Tagi> IdOceny5s { get; set; } = new List<Tagi>();

    public virtual ICollection<Uzytkownik> IdUzytkowniks { get; set; } = new List<Uzytkownik>();

   



}
