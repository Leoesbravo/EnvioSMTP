namespace PL.Models
{
    public class Movie
    {
        public int IdMovie { get; set; }
        public string Nombre { get; set; }
        public string Poster { get; set; }
        public List<object> Movies { get; set; }
    }
}
