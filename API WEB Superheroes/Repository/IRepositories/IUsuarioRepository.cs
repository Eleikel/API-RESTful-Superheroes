using API_WEB_Superheroes.Models;
using API_WEB_Superheroes.Repository.Repositories;

namespace API_WEB_Superheroes.Repository.IRepositories
{
    public interface IUsuarioRepository : IGenericRepository<UsuarioRepository>
    {
        Task<ICollection<Usuario>> ObtenerUsuarios();
        Task<Usuario> ObtenerUsuario(int UserId);
        Task<bool> ExisteUsuario(string user);
        Task<Usuario> Registrar(Usuario user, string password);
        Task<Usuario> Loguearse(string user, string password);
    }
}
