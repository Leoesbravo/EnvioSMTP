namespace PL.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; }
        public byte[] Password { get; set; }
    }
}
