using System.ComponentModel.DataAnnotations;

namespace API_WEB_Superheroes.Models.Dto
{
    public class UsuarioAutenticacionDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Usuario es obligatorio")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener 4 caracteres como minimo")]
        public string Password { get; set; }

    }
}
