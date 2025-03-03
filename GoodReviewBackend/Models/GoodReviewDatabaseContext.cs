using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GoodReviewBackend.Models;

public partial class GoodReviewDatabaseContext : DbContext
{
    public GoodReviewDatabaseContext()
    {
    }

    public GoodReviewDatabaseContext(DbContextOptions<GoodReviewDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Gatunek> Gatuneks { get; set; }

    public virtual DbSet<Komentarz> Komentarzs { get; set; }

    public virtual DbSet<Ksiazka> Ksiazkas { get; set; }

    public virtual DbSet<Listum> Lista { get; set; }

    public virtual DbSet<Ocena> Ocenas { get; set; }

    public virtual DbSet<Polecenium> Polecenia { get; set; }

    public virtual DbSet<Recenzja> Recenzjas { get; set; }

    public virtual DbSet<Rola> Rolas { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusNazwa> StatusNazwas { get; set; }

    public virtual DbSet<Tagi> Tagis { get; set; }

    public virtual DbSet<TypAutorstwa> TypAutorstwas { get; set; }

    public virtual DbSet<Udzial> Udzials { get; set; }

    public virtual DbSet<Uzytkownik> Uzytkowniks { get; set; }

    public virtual DbSet<Wydawnictwo> Wydawnictwos { get; set; }

    public virtual DbSet<Znajomi> Znajomis { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-GOM89BG\\SQLEXPRESS;Database=GoodReviewDatabase;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutora).IsClustered(false);

            entity.ToTable("AUTOR");

            entity.Property(e => e.IdAutora).HasColumnName("ID_AUTORA");
            entity.Property(e => e.DataSmierci)
                .HasColumnType("datetime")
                .HasColumnName("DATA_SMIERCI");
            entity.Property(e => e.DataUrodzenia)
                .HasColumnType("datetime")
                .HasColumnName("DATA_URODZENIA");
            entity.Property(e => e.ImieAutora)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMIE_AUTORA");
            entity.Property(e => e.NazwiskoAutora)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWISKO_AUTORA");
            entity.Property(e => e.Opis)
                .HasColumnType("text")
                .HasColumnName("OPIS");
            entity.Property(e => e.Pseudonim)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PSEUDONIM");
            entity.Property(e => e.Wiek).HasColumnName("WIEK");
        });


        modelBuilder.Entity<Gatunek>(entity =>
        {
            entity.HasKey(e => e.IdGatunku).IsClustered(false);

            entity.ToTable("GATUNEK");

            entity.Property(e => e.IdGatunku).HasColumnName("ID_GATUNKU");
            entity.Property(e => e.NazwaGatunku)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWA_GATUNKU");

            entity.HasMany(d => d.IdKsiazkas)
                .WithMany(p => p.IdGatunkus)
                .UsingEntity<Dictionary<string, object>>(
                    "Gatunkowosc", 
                    r => r.HasOne<Ksiazka>().WithMany().HasForeignKey("IdKsiazka"),
                    l => l.HasOne<Gatunek>().WithMany().HasForeignKey("IdGatunku"),
                    j =>
                    {
                        j.HasKey("IdGatunku", "IdKsiazka").IsClustered(false);
                        j.ToTable("GATUNKOWOSC");
                        j.HasIndex(new[] { "IdKsiazka" }, "GATUNKOWOSC2_FK");
                        j.HasIndex(new[] { "IdGatunku" }, "GATUNKOWOSC_FK");
                        j.IndexerProperty<int>("IdGatunku").HasColumnName("ID_GATUNKU");
                        j.IndexerProperty<int>("IdKsiazka").HasColumnName("ID_KSIAZKA");
                    });

            entity.HasMany(d => d.IdUzytkowniks)
                .WithMany(p => p.IdGatunkus)
                .UsingEntity<Dictionary<string, object>>(
                    "UlubioneGatunki",  
                    r => r.HasOne<Uzytkownik>().WithMany().HasForeignKey("IdUzytkownik"),
                    l => l.HasOne<Gatunek>().WithMany().HasForeignKey("IdGatunku"),
                    j =>
                    {
                        j.HasKey("IdGatunku", "IdUzytkownik").IsClustered(false);
                        j.ToTable("ULUBIONE_GATUNKI");
                        j.HasIndex(new[] { "IdUzytkownik" }, "ULUBIONE_GATUNKI2_FK");
                        j.HasIndex(new[] { "IdGatunku" }, "ULUBIONE_GATUNKI_FK");
                        j.IndexerProperty<int>("IdGatunku").HasColumnName("ID_GATUNKU");
                        j.IndexerProperty<int>("IdUzytkownik").HasColumnName("ID_UZYTKOWNIK");
                    });
        });

        modelBuilder.Entity<Komentarz>(entity =>
        {
            entity.HasKey(e => e.IdOceny3).IsClustered(false);

            entity.ToTable("KOMENTARZ");

            entity.HasIndex(e => e.IdRecenzji, "KOMENTARZE_RECENZJI_FK");

            entity.HasIndex(e => e.IdUzytkownik, "KOMENTARZE_UZYTKOWNIKA_FK");

            entity.Property(e => e.IdOceny3).HasColumnName("ID_OCENY3");
            entity.Property(e => e.IdRecenzji).HasColumnName("ID_RECENZJI");
            entity.Property(e => e.IdUzytkownik).HasColumnName("ID_UZYTKOWNIK");
            entity.Property(e => e.TrescKomentarza)
                .HasColumnType("text")
                .HasColumnName("TRESC_KOMENTARZA");

            entity.HasOne(d => d.IdRecenzjiNavigation).WithMany(p => p.Komentarzs)
                .HasForeignKey(d => d.IdRecenzji)
                .HasConstraintName("FK_KOMENTAR_KOMENTARZ_RECENZJA");

            entity.HasOne(d => d.IdUzytkownikNavigation).WithMany(p => p.Komentarzs)
                .HasForeignKey(d => d.IdUzytkownik)
                .HasConstraintName("FK_KOMENTAR_KOMENTARZ_UZYTKOWN");
        });

        modelBuilder.Entity<Ksiazka>(entity =>
        {
            entity.HasKey(e => e.IdKsiazka).IsClustered(false);

            entity.ToTable("KSIAZKA");

            entity.HasIndex(e => e.IdWydawnictwa, "WYDANE_PRZEZ_FK");

            entity.Property(e => e.IdKsiazka).HasColumnName("ID_KSIAZKA");
            entity.Property(e => e.IdWydawnictwa).HasColumnName("ID_WYDAWNICTWA");
            entity.Property(e => e.IloscStron).HasColumnName("ILOSC_STRON");
            entity.Property(e => e.Isbn)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.LiczbaOcen).HasColumnName("LICZBA_OCEN");
            entity.Property(e => e.Okladka)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("OKLADKA");
            entity.Property(e => e.Opis)
                .HasColumnType("text")
                .HasColumnName("OPIS");
            entity.Property(e => e.RokWydania)
                .HasColumnType("datetime")
                .HasColumnName("ROK_WYDANIA");
            entity.Property(e => e.Tytul)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TYTUL");

            entity.HasOne(d => d.IdWydawnictwaNavigation).WithMany(p => p.Ksiazkas)
                .HasForeignKey(d => d.IdWydawnictwa)
                .HasConstraintName("FK_KSIAZKA_WYDANE_PR_WYDAWNIC");

            entity.HasMany(d => d.IdListies).WithMany(p => p.IdKsiazkas)
                .UsingEntity<Dictionary<string, object>>(
                    "DodawanieDoList",
                    r => r.HasOne<Listum>().WithMany()
                        .HasForeignKey("IdListy")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DODAWANI_DODAWANIE_LISTA"),
                    l => l.HasOne<Ksiazka>().WithMany()
                        .HasForeignKey("IdKsiazka")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DODAWANI_DODAWANIE_KSIAZKA"),
                    j =>
                    {
                        j.HasKey("IdKsiazka", "IdListy").IsClustered(false);
                        j.ToTable("DODAWANIE_DO_LIST");
                        j.HasIndex(new[] { "IdListy" }, "DODAWANIE_DO_LIST2_FK");
                        j.HasIndex(new[] { "IdKsiazka" }, "DODAWANIE_DO_LIST_FK");
                        j.IndexerProperty<int>("IdKsiazka").HasColumnName("ID_KSIAZKA");
                        j.IndexerProperty<int>("IdListy").HasColumnName("ID_LISTY");
                    });

            entity.HasMany(d => d.IdUzytkowniks).WithMany(p => p.IdKsiazkas)
                .UsingEntity<Dictionary<string, object>>(
                    "HistoriaPrzegladanium",
                    r => r.HasOne<Uzytkownik>().WithMany()
                        .HasForeignKey("IdUzytkownik")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_HISTORIA_HISTORIA__UZYTKOWN"),
                    l => l.HasOne<Ksiazka>().WithMany()
                        .HasForeignKey("IdKsiazka")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_HISTORIA_HISTORIA__KSIAZKA"),
                    j =>
                    {
                        j.HasKey("IdKsiazka", "IdUzytkownik").IsClustered(false);
                        j.ToTable("HISTORIA_PRZEGLADANIA");
                        j.HasIndex(new[] { "IdUzytkownik" }, "HISTORIA_PRZEGLADANIA2_FK");
                        j.HasIndex(new[] { "IdKsiazka" }, "HISTORIA_PRZEGLADANIA_FK");
                        j.IndexerProperty<int>("IdKsiazka").HasColumnName("ID_KSIAZKA");
                        j.IndexerProperty<int>("IdUzytkownik").HasColumnName("ID_UZYTKOWNIK");
                    });
        });

        modelBuilder.Entity<Listum>(entity =>
        {
            entity.HasKey(e => e.IdListy).IsClustered(false);

            entity.ToTable("LISTA");

            entity.HasIndex(e => e.IdUzytkownik, "TWORZENIA_LIST_FK");

            entity.Property(e => e.IdListy).HasColumnName("ID_LISTY");
            entity.Property(e => e.IdUzytkownik).HasColumnName("ID_UZYTKOWNIK");
            entity.Property(e => e.IloscElementow).HasColumnName("ILOSC_ELEMENTOW");
            entity.Property(e => e.NazwaListy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWA_LISTY");
            entity.Property(e => e.OpisListy)
                .HasColumnType("text")
                .HasColumnName("OPIS_LISTY");
            entity.Property(e => e.Prywatna).HasColumnName("PRYWATNA");

            entity.HasOne(d => d.IdUzytkownikNavigation).WithMany(p => p.Lista)
                .HasForeignKey(d => d.IdUzytkownik)
                .HasConstraintName("FK_LISTA_TWORZENIA_UZYTKOWN");
        });

        modelBuilder.Entity<Ocena>(entity =>
        {
            entity.HasKey(e => e.IdOceny).IsClustered(false);

            entity.ToTable("OCENA");

            entity.HasIndex(e => e.IdKsiazka, "OCENIANIE_KSIAZEK_FK");

            entity.HasIndex(e => e.IdUzytkownik, "OCENY_WYSTAWIONE_FK");

            entity.Property(e => e.IdOceny).HasColumnName("ID_OCENY");
            entity.Property(e => e.DataOceny)
                .HasColumnType("datetime")
                .HasColumnName("DATA_OCENY");
            entity.Property(e => e.IdKsiazka).HasColumnName("ID_KSIAZKA");
            entity.Property(e => e.IdUzytkownik).HasColumnName("ID_UZYTKOWNIK");
            entity.Property(e => e.WartoscOceny).HasColumnName("WARTOSC_OCENY");

            entity.HasOne(d => d.IdKsiazkaNavigation).WithMany(p => p.Ocenas)
                .HasForeignKey(d => d.IdKsiazka)
                .HasConstraintName("FK_OCENA_OCENIANIE_KSIAZKA");

            entity.HasOne(d => d.IdUzytkownikNavigation).WithMany(p => p.Ocenas)
                .HasForeignKey(d => d.IdUzytkownik)
                .HasConstraintName("FK_OCENA_OCENY_WYS_UZYTKOWN");
        });

        modelBuilder.Entity<Polecenium>(entity =>
        {
            entity.HasKey(e => e.IdPolecenia).IsClustered(false);

            entity.ToTable("POLECENIA");

            entity.HasIndex(e => e.IdKsiazka, "POLECENIE_KSIAZKI_FK");

            entity.HasIndex(e => e.IdZnajomosci, "POLECENIE_ZNAJOMEMU_FK");

            entity.Property(e => e.IdPolecenia).HasColumnName("ID_POLECENIA");
            entity.Property(e => e.DataPolecenia)
                .HasColumnType("datetime")
                .HasColumnName("DATA_POLECENIA");
            entity.Property(e => e.IdKsiazka).HasColumnName("ID_KSIAZKA");
            entity.Property(e => e.IdZnajomosci).HasColumnName("ID_ZNAJOMOSCI");
            entity.Property(e => e.TrescPolecenia)
                .HasColumnType("text")
                .HasColumnName("TRESC_POLECENIA");

            entity.HasOne(d => d.IdKsiazkaNavigation).WithMany(p => p.Polecenia)
                .HasForeignKey(d => d.IdKsiazka)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POLECENI_POLECENIE_KSIAZKA");

            entity.HasOne(d => d.IdZnajomosciNavigation).WithMany(p => p.Polecenia)
                .HasForeignKey(d => d.IdZnajomosci)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POLECENI_POLECENIE_ZNAJOMI");
        });

        modelBuilder.Entity<Recenzja>(entity =>
        {
            entity.HasKey(e => e.IdRecenzji).IsClustered(false);

            entity.ToTable("RECENZJA");

            entity.HasIndex(e => e.IdKsiazka, "RECENZJE_KSIAZKI_FK");

            entity.HasIndex(e => e.IdUzytkownik, "RECENZJE_UZYTKOWNIKA_FK");

            entity.Property(e => e.IdRecenzji).HasColumnName("ID_RECENZJI");
            entity.Property(e => e.DataRecenzji)
                .HasColumnType("datetime")
                .HasColumnName("DATA_RECENZJI");
            entity.Property(e => e.IdKsiazka).HasColumnName("ID_KSIAZKA");
            entity.Property(e => e.IdUzytkownik).HasColumnName("ID_UZYTKOWNIK");
            entity.Property(e => e.PolubieniaRecenzji).HasColumnName("POLUBIENIA_RECENZJI");
            entity.Property(e => e.TrescRecenzji)
                .HasColumnType("text")
                .HasColumnName("TRESC_RECENZJI");

            entity.HasOne(d => d.IdKsiazkaNavigation).WithMany(p => p.Recenzjas)
                .HasForeignKey(d => d.IdKsiazka)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RECENZJA_RECENZJE__KSIAZKA");

            entity.HasOne(d => d.IdUzytkownikNavigation).WithMany(p => p.Recenzjas)
                .HasForeignKey(d => d.IdUzytkownik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RECENZJA_RECENZJE__UZYTKOWN");
        });

        modelBuilder.Entity<Rola>(entity =>
        {
            entity.HasKey(e => e.IdOceny2).IsClustered(false);

            entity.ToTable("ROLA");

            entity.Property(e => e.IdOceny2).HasColumnName("ID_OCENY2");
            entity.Property(e => e.NazwaRoli)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWA_ROLI");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatusu).IsClustered(false);

            entity.ToTable("STATUS");

            entity.HasIndex(e => e.IdUzytkownik, "STATUS_KSIAZEK_UZYTKOWNIK_FK");

            entity.HasIndex(e => e.IdKsiazka, "STATUS_KSIAZKI_FK");

            entity.HasIndex(e => e.IdStatusNazwa, "WYBIERANIE_STATUSU_FK");

            entity.Property(e => e.IdStatusu).HasColumnName("ID_STATUSU");
            entity.Property(e => e.DataStatusu)
                .HasColumnType("datetime")
                .HasColumnName("DATA_STATUSU");
            entity.Property(e => e.IdKsiazka).HasColumnName("ID_KSIAZKA");
            entity.Property(e => e.IdStatusNazwa).HasColumnName("ID_STATUS_NAZWA");
            entity.Property(e => e.IdUzytkownik).HasColumnName("ID_UZYTKOWNIK");
            entity.Property(e => e.KomentarzStatusu)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("KOMENTARZ_STATUSU");

            entity.HasOne(d => d.IdKsiazkaNavigation).WithMany(p => p.Statuses)
                .HasForeignKey(d => d.IdKsiazka)
                .HasConstraintName("FK_STATUS_STATUS_KS_KSIAZKA");

            entity.HasOne(d => d.IdStatusNazwaNavigation).WithMany(p => p.Statuses)
                .HasForeignKey(d => d.IdStatusNazwa)
                .HasConstraintName("FK_STATUS_WYBIERANI_STATUS_N");

            entity.HasOne(d => d.IdUzytkownikNavigation).WithMany(p => p.Statuses)
                .HasForeignKey(d => d.IdUzytkownik)
                .HasConstraintName("FK_STATUS_STATUS_KS_UZYTKOWN");
        });

        modelBuilder.Entity<StatusNazwa>(entity =>
        {
            entity.HasKey(e => e.IdStatusNazwa).IsClustered(false);

            entity.ToTable("STATUS_NAZWA");

            entity.Property(e => e.IdStatusNazwa).HasColumnName("ID_STATUS_NAZWA");
            entity.Property(e => e.NazwaStatusu)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWA_STATUSU");
        });

        modelBuilder.Entity<Tagi>(entity =>
        {
            entity.HasKey(e => e.IdOceny5).IsClustered(false);

            entity.ToTable("TAGI");

            entity.Property(e => e.IdOceny5).HasColumnName("ID_OCENY5");
            entity.Property(e => e.NazwaTagu)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWA_TAGU");

            entity.HasMany(d => d.IdKsiazkas).WithMany(p => p.IdOceny5s)
                .UsingEntity<Dictionary<string, object>>(
                    "TagowanieKsiazek",
                    r => r.HasOne<Ksiazka>().WithMany()
                        .HasForeignKey("IdKsiazka")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TAGOWANI_TAGOWANIE_KSIAZKA"),
                    l => l.HasOne<Tagi>().WithMany()
                        .HasForeignKey("IdOceny5")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TAGOWANI_TAGOWANIE_TAGI"),
                    j =>
                    {
                        j.HasKey("IdOceny5", "IdKsiazka").IsClustered(false);
                        j.ToTable("TAGOWANIE_KSIAZEK");
                        j.HasIndex(new[] { "IdKsiazka" }, "TAGOWANIE_KSIAZEK2_FK");
                        j.HasIndex(new[] { "IdOceny5" }, "TAGOWANIE_KSIAZEK_FK");
                        j.IndexerProperty<int>("IdOceny5").HasColumnName("ID_OCENY5");
                        j.IndexerProperty<int>("IdKsiazka").HasColumnName("ID_KSIAZKA");
                    });
        });

        modelBuilder.Entity<TypAutorstwa>(entity =>
        {
            entity.HasKey(e => e.IdTypu).IsClustered(false);

            entity.ToTable("TYP_AUTORSTWA");

            entity.Property(e => e.IdTypu).HasColumnName("ID_TYPU");
            entity.Property(e => e.NazwaTypu)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWA_TYPU");
        });

        modelBuilder.Entity<Udzial>(entity =>
        {
            entity.HasKey(e => e.IdUdzialu).IsClustered(false);

            entity.ToTable("UDZIAL");

            entity.HasIndex(e => e.IdAutora, "ILOSC_UDZIALU_AUTORA_FK");

            entity.HasIndex(e => e.IdTypu, "TYPOWANIE_AUTORSTWA_FK");

            entity.HasIndex(e => e.IdKsiazka, "UDZIAL_W_KSIAZCE_FK");

            entity.Property(e => e.IdUdzialu).HasColumnName("ID_UDZIALU");
            entity.Property(e => e.IdAutora).HasColumnName("ID_AUTORA");
            entity.Property(e => e.IdKsiazka).HasColumnName("ID_KSIAZKA");
            entity.Property(e => e.IdTypu).HasColumnName("ID_TYPU");
            entity.Property(e => e.WartoscUdzialu).HasColumnName("WARTOSC_UDZIALU");

            entity.HasOne(d => d.IdAutoraNavigation).WithMany(p => p.Udzials)
                .HasForeignKey(d => d.IdAutora)
                .HasConstraintName("FK_UDZIAL_ILOSC_UDZ_AUTOR");

            entity.HasOne(d => d.IdKsiazkaNavigation).WithMany(p => p.Udzials)
                .HasForeignKey(d => d.IdKsiazka)
                .HasConstraintName("FK_UDZIAL_UDZIAL_W__KSIAZKA");

            entity.HasOne(d => d.IdTypuNavigation).WithMany(p => p.Udzials)
                .HasForeignKey(d => d.IdTypu)
                .HasConstraintName("FK_UDZIAL_TYPOWANIE_TYP_AUTO");
        });

        modelBuilder.Entity<Uzytkownik>(entity =>
        {
            entity.HasKey(e => e.IdUzytkownik).IsClustered(false);

            entity.ToTable("UZYTKOWNIK");

            entity.HasIndex(e => e.IdOceny2, "ROLA_UZYTKOWNIKA_FK");

            entity.Property(e => e.IdUzytkownik).HasColumnName("ID_UZYTKOWNIK");
            entity.Property(e => e.DataRejestracji)
                .HasColumnType("datetime")
                .HasColumnName("DATA_REJESTRACJI");
            entity.Property(e => e.EMail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("E_MAIL");
            entity.Property(e => e.Haslo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HASLO");
            entity.Property(e => e.IdOceny2).HasColumnName("ID_OCENY2");
            entity.Property(e => e.IloscOcen).HasColumnName("ILOSC_OCEN");
            entity.Property(e => e.IloscRecenzji).HasColumnName("ILOSC_RECENZJI");
            entity.Property(e => e.Imie)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IMIE");
            entity.Property(e => e.Nazwisko)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAZWISKO");
            entity.Property(e => e.Opis)
                .HasColumnType("text")
                .HasColumnName("OPIS");
            entity.Property(e => e.OstaniaAktywnosc)
                .HasColumnType("datetime")
                .HasColumnName("OSTANIA_AKTYWNOSC");
            entity.Property(e => e.Zdjecie)
                .HasColumnType("text")
                .HasColumnName("ZDJECIE");
            entity.Property(e => e.Znajomi).HasColumnName("ZNAJOMI");

            // Konfiguracja kolumny DataUrodzenia
            entity.Property(e => e.DataUrodzenia)
                .HasColumnType("date")  // Ustawienie typu kolumny na "date"
                .HasColumnName("DataUrodzenia"); // Ustawienie nazwy kolumny w bazie danych

            entity.HasOne(d => d.IdOceny2Navigation).WithMany(p => p.Uzytkowniks)
                .HasForeignKey(d => d.IdOceny2)
                .HasConstraintName("FK_UZYTKOWN_ROLA_UZYT_ROLA");
        });


        modelBuilder.Entity<Wydawnictwo>(entity =>
        {
            entity.HasKey(e => e.IdWydawnictwa).IsClustered(false);

            entity.ToTable("WYDAWNICTWO");

            entity.Property(e => e.IdWydawnictwa).HasColumnName("ID_WYDAWNICTWA");
            entity.Property(e => e.AdresSiedziby)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADRES_SIEDZIBY");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAZWA");
            entity.Property(e => e.StronaInternetowa)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("STRONA_INTERNETOWA");
        });

        modelBuilder.Entity<Znajomi>(entity =>
        {
            entity.HasKey(e => e.IdZnajomosci).IsClustered(false);

            entity.ToTable("ZNAJOMI");

            entity.HasIndex(e => e.UzyIdUzytkownik, "RELACJA_FK");

            entity.HasIndex(e => e.IdUzytkownik, "ZNAJOMOSC_FK");

            entity.Property(e => e.IdZnajomosci).HasColumnName("ID_ZNAJOMOSCI");
            entity.Property(e => e.DataZnajomosci)
                .HasColumnType("datetime")
                .HasColumnName("DATA_ZNAJOMOSCI");
            entity.Property(e => e.IdUzytkownik).HasColumnName("ID_UZYTKOWNIK");
            entity.Property(e => e.StatusZnajomosci)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STATUS_ZNAJOMOSCI");
            entity.Property(e => e.UzyIdUzytkownik).HasColumnName("UZY_ID_UZYTKOWNIK");

            entity.HasOne(d => d.IdUzytkownikNavigation).WithMany(p => p.ZnajomiIdUzytkownikNavigations)
                .HasForeignKey(d => d.IdUzytkownik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ZNAJOMI_ZNAJOMOSC_UZYTKOWN");

            entity.HasOne(d => d.UzyIdUzytkownikNavigation).WithMany(p => p.ZnajomiUzyIdUzytkownikNavigations)
                .HasForeignKey(d => d.UzyIdUzytkownik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ZNAJOMI_RELACJA_UZYTKOWN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
