namespace API_WEB_Superheroes.Models
{
    public class Usuario
    { 
        public int Id { get; set; }
        public string UsuarioA { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set;}
        public byte[] PasswordSalt { get; set; }

    }
}
