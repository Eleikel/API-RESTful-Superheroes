using System.ComponentModel.DataAnnotations;

namespace API_WEB_Superheroes.Models.Dto
{
    //Loguearse
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; }
    }
}
