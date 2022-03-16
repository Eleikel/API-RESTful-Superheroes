using API_WEB_Superheroes.Repository.IConfiguration;
using API_WEB_Superheroes.Repository.IRepositories;
using API_WEB_Superheroes.Repository.Repositories;

namespace API_WEB_Superheroes.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public readonly ApplicationDbContext _context;
        public ISuperHeroeRepository SuperHeroe { get; private set;}
        public IUsuarioRepository Usuario { get; private set;}

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            SuperHeroe = new SuperHeroeRepository(context);
            Usuario = new UsuarioRepository(context);
        }

        //Guardar datos en base datos
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
