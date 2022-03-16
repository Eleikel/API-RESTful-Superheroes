using API_WEB_Superheroes.Models;
using API_WEB_Superheroes.Models.Dto;
using API_WEB_Superheroes.Repository.IConfiguration;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_WEB_Superheroes.Controllers
{
    [Authorize]
    [Route("api/Usuario")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiUsuario")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class UsuarioController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;// Generar Token


        public UsuarioController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// Obtener todos los usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var listUsers = await _unitOfWork.Usuario.ObtenerUsuarios();

            var listUsersDto = new List<UsuarioDto>();

            foreach (var user in listUsers)
            {
                listUsersDto.Add(_mapper.Map<UsuarioDto>(user));
            }

            return Ok(listUsersDto);
        }

        /// <summary>
        /// Obtener usuario por ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:int}", Name = "ObtenerUsuario")]
        [ProducesResponseType(200, Type = typeof(UsuarioDto))]  // El 'ProducesResponseType' es importante ponerlo
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> ObtenerUsuario(int userId)
        {
            var itemUser = await _unitOfWork.Usuario.ObtenerUsuario(userId);

            if (itemUser == null)
            {
                return NotFound();
            }

            var itemUserDto = _mapper.Map<UsuarioDto>(itemUser);
            return Ok(itemUserDto);
        }
        /// <summary>
        /// Registrar Usuario
        /// </summary>
        /// <param name="usuarioAutenticacion"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RegistrarUsuario")]
        [ProducesResponseType(201, Type = typeof(UsuarioAutenticacionDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarUsuario(UsuarioAutenticacionDto usuarioAutenticacion)
        {
            usuarioAutenticacion.Usuario = usuarioAutenticacion.Usuario.ToLower();

            if (await _unitOfWork.Usuario.ExisteUsuario(usuarioAutenticacion.Usuario))
            {
                return BadRequest("The User already exist");
            }

            var usuarioACrear = new Usuario
            {
                UsuarioA = usuarioAutenticacion.Usuario
            };

            var usuarioRegistrado = await _unitOfWork.Usuario.Registrar(usuarioACrear, usuarioAutenticacion.Password);
            await _unitOfWork.CompleteAsync();

            return Ok(usuarioRegistrado);
        }


        /// <summary>
        /// Loguearse / Ingresar
        /// </summary>
        /// <param name="usuarioLoginDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(UsuarioLoginDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuarioLoguearse = await _unitOfWork.Usuario.Loguearse(usuarioLoginDto.Usuario, usuarioLoginDto.Contraseña);

            if (usuarioLoginDto == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioLoguearse.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarioLoguearse.UsuarioA.ToString())
            };


            //Token Generation
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

    }

}
