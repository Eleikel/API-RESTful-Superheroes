using API_WEB_Superheroes.Repository.IRepositories;

namespace API_WEB_Superheroes.Repository.IConfiguration
{
    public interface IUnitOfWork
    {
        ISuperHeroeRepository SuperHeroe { get; }
        IUsuarioRepository Usuario { get; }
        Task CompleteAsync();
    }
}
