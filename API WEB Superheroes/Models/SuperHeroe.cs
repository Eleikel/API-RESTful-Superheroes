namespace API_WEB_Superheroes.Models
{
    public class SuperHeroe
    {
        public int Id { get; set; }
        public string NombreSuperHeroe { get; set; } = string.Empty;  
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Poderes { get; set; } = string.Empty;
        public string LugarNacimiento { get; set; } = string.Empty;
    }
}
