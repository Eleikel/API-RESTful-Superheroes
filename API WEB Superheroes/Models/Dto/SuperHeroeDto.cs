using System.ComponentModel.DataAnnotations;

namespace API_WEB_Superheroes.Models.Dto
{
    public class SuperHeroeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo NombreSuperHeroe es obligatorio.")]
        public string NombreSuperHeroe { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Poderes es obligatorio.")]
        public string Poderes { get; set; } = string.Empty;
        public string LugarNacimiento { get; set; } = string.Empty;
    }
}
