using API_WEB_Superheroes.Models;
using API_WEB_Superheroes.Repository.Repositories;

namespace API_WEB_Superheroes.Repository.IRepositories
{
    public interface ISuperHeroeRepository : IGenericRepository<SuperHeroe>
    {
        Task<IEnumerable<SuperHeroe>> BuscarSuperHeroe(string name);

    }
}
