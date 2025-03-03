public class RecenzjaDTO
{
    public int IdRecenzji { get; set; }
    public int IdKsiazka { get; set; }

    public int IdUzytkownik { get; set; }
    public string? TrescRecenzji { get; set; }
    public DateTime DataRecenzji { get; set; }
    public int PolubieniaRecenzji { get; set; }

    public string? UzytkownikImie { get; set; }
    public string? UzytkownikNazwisko { get; set; }
    public string? UzytkownikZdjecie { get; set; }

    public string?  KsiazkaTytul { get; set; }
    public string? KsiazkaOkladka { get; set; }
}
