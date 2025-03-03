public class KsiazkaDto
{
    public int? IdKsiazka { get; set; }         
    public string Tytul { get; set; }            
    public string Opis { get; set; }            
    public string Okladka { get; set; }          
    public DateTime? RokWydania { get; set; }    
    public int? IloscStron { get; set; }        
    public string Isbn { get; set; }           
    public int? IdWydawnictwa { get; set; }      
    public List<int> GatunkiIds { get; set; }    
    public List<int> AutorzyIds { get; set; }    
}
