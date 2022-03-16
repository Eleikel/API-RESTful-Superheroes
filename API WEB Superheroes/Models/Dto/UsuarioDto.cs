namespace API_WEB_Superheroes.Models.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string UserA { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
