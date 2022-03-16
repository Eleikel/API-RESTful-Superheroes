using API_WEB_Superheroes.Models;
using API_WEB_Superheroes.Models.Dto;
using AutoMapper;

namespace API_WEB_Superheroes.SuperHeroe_Mapper
{
    public class SuperHeroeMappers : Profile
    {
        public SuperHeroeMappers()
        {
            CreateMap<SuperHeroe, SuperHeroeDto>().ReverseMap();
            CreateMap<SuperHeroe, ActualizarSuperHeroeDto>().ReverseMap();
            CreateMap<SuperHeroe, CrearSuperHeroeDto>().ReverseMap();

            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioAutenticacionDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();

        }

    }
}
