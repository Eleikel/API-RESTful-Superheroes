using API_WEB_Superheroes.Models;
using API_WEB_Superheroes.Models.Dto;
using API_WEB_Superheroes.Repository.IConfiguration;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_WEB_Superheroes.Controllers
{
    [Authorize]
    [Route("api/SuperHeroe")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiSuperHeroe")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class SuperHeroeController : Controller
    {
        private readonly IUnitOfWork _uniOfWork;
        private readonly IMapper _mapper;


        public SuperHeroeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uniOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener lista de SuperHeroes
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SuperHeroe>>> Obtener()
        {
            var listaSuperHeroes = await _uniOfWork.SuperHeroe.ObtenerTodos();

            var SuperHeroesDto = new List<SuperHeroeDto>();

            foreach (var item in listaSuperHeroes)
            {
                SuperHeroesDto.Add(_mapper.Map<SuperHeroeDto>(item));
            }

            return Ok(SuperHeroesDto);
        }
        /// <summary>
        /// Obtener SuperHeroe por ID
        /// </summary>
        /// <param name="superHeroeId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{superHeroeId:int}", Name = "ObtenerPorId")]
        [ProducesResponseType(200, Type = typeof(SuperHeroeDto))]  // El 'ProducesResponseType' es importante ponerlo
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<SuperHeroe>>> ObtenerPorId(int superHeroeId)
        {
            var superHeroe = await _uniOfWork.SuperHeroe.ObtenerPorId(superHeroeId);

            if (superHeroe == null)
            {
                return NotFound();
            }

            var superHeroeDto = _mapper.Map<SuperHeroeDto>(superHeroe);

            return Ok(superHeroeDto);
        }

        /// <summary>
        /// Crear un nuevo SuperHeroe
        /// </summary>
        /// <param name="crearSuperHeroeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CrearSuperHeroeDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CrearSuperHeroe([FromBody] CrearSuperHeroeDto crearSuperHeroeDto)
        {
            if (crearSuperHeroeDto == null)
            {
                return BadRequest(ModelState);
            }

            if (await _uniOfWork.SuperHeroe.Existe(crearSuperHeroeDto.NombreSuperHeroe))
            {
                ModelState.AddModelError("", $"El Super Heroe {crearSuperHeroeDto.NombreSuperHeroe} ya existe.");
                return StatusCode(404, ModelState);
            }

            var superHeroe = _mapper.Map<SuperHeroe>(crearSuperHeroeDto);

            if (!await _uniOfWork.SuperHeroe.Crear(superHeroe))
            {
                ModelState.AddModelError("", $"Algo salio mal agregando el resgistro {superHeroe.NombreSuperHeroe}.");
                return StatusCode(500, ModelState);
            }

            await _uniOfWork.CompleteAsync();

            return CreatedAtRoute("ObtenerPorId", new { superHeroeId = superHeroe.Id }, superHeroe);
        }


        /// <summary>
        /// Actualizar un SuperHeroe 
        /// </summary>
        /// <param name="superHeroeId"></param>
        /// <param name="actualizarSuperHeroeDto"></param>
        /// <returns></returns>
        [HttpPatch("superHeroeId:int", Name = "ActualizarSuperHeroe")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SuperHeroe>>> ActualizarSuperHeroe(int superHeroeId, [FromBody] ActualizarSuperHeroeDto actualizarSuperHeroeDto)
        {
            if (actualizarSuperHeroeDto == null || actualizarSuperHeroeDto.Id != superHeroeId)
            {
                return BadRequest(ModelState);
            }

            var superHeroe = _mapper.Map<SuperHeroe>(actualizarSuperHeroeDto);

            if (!await _uniOfWork.SuperHeroe.Actualizar(superHeroe))
            {
                ModelState.AddModelError("", $"Algo salio mal agregando el resgistro {superHeroe.NombreSuperHeroe}.");
                return StatusCode(500, ModelState);
            }

            await _uniOfWork.CompleteAsync();

            return Content("Se actualizo correctamente");
        }

        /// <summary>
        /// Eliminar un SuperHeroe
        /// </summary>
        /// <param name="superHeroeId"></param>
        /// <returns></returns>
        [HttpDelete("{superHeroeId:int}", Name = "EliminarSuperHeroe")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<SuperHeroe>>> EliminarSuperHeroe(int superHeroeId)
        {
            if (!await _uniOfWork.SuperHeroe.Existe(superHeroeId))
            {
                return NotFound();
            }

            var superHeroe = await _uniOfWork.SuperHeroe.ObtenerPorId(superHeroeId);

            if (!await _uniOfWork.SuperHeroe.Eliminar(superHeroe))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {superHeroe.NombreSuperHeroe}");
                return StatusCode(500, ModelState);
            }

            await _uniOfWork.CompleteAsync();

            return Content($"Se elimino el registro {superHeroe.NombreSuperHeroe}");
        }

        /// <summary>
        /// Buscar SuperHeroe por nombre
        /// </summary>
        /// <param name="nombreSuperHeroe"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [HttpGet("BuscarSuperHeroe")]
        public async Task<IActionResult> BuscarSuperHeroe(string nombreSuperHeroe)
        {
            try
            {
                var resultado = await _uniOfWork.SuperHeroe.BuscarSuperHeroe(nombreSuperHeroe);

                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
