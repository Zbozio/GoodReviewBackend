public class KsiazkaDto
{
    public int? IdKsiazka { get; set; }          // Identyfikator (null dla nowej książki)
    public string Tytul { get; set; }            // Tytuł książki
    public string Opis { get; set; }             // Opis książki
    public string Okladka { get; set; }          // URL do okładki
    public DateTime? RokWydania { get; set; }    // Rok wydania książki
    public int? IloscStron { get; set; }         // Liczba stron
    public string Isbn { get; set; }             // Numer ISBN
    public int? IdWydawnictwa { get; set; }      // Id wydawnictwa
    public List<int> GatunkiIds { get; set; }    // Lista ID gatunków
    public List<int> AutorzyIds { get; set; }    // Lista ID autorów
}
