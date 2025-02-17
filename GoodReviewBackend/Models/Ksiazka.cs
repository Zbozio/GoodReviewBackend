using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public virtual Wydawnictwo? IdWydawnictwaNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Ocena> Ocenas { get; set; } = new List<Ocena>();
    [JsonIgnore]
    public virtual ICollection<Polecenium> Polecenia { get; set; } = new List<Polecenium>();
    [JsonIgnore]
    public virtual ICollection<Recenzja> Recenzjas { get; set; } = new List<Recenzja>();
    [JsonIgnore]
    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();
    [JsonIgnore]
    public virtual ICollection<Udzial> Udzials { get; set; } = new List<Udzial>();
    [JsonIgnore]

    public virtual ICollection<Gatunek> IdGatunkus { get; set; } = new List<Gatunek>();
    [JsonIgnore]
    public virtual ICollection<Listum> IdListies { get; set; } = new List<Listum>();
    [JsonIgnore]

    public virtual ICollection<Tagi> IdOceny5s { get; set; } = new List<Tagi>();
    [JsonIgnore]

    public virtual ICollection<Uzytkownik> IdUzytkowniks { get; set; } = new List<Uzytkownik>();

   



}
