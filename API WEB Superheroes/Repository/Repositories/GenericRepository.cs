using API_WEB_Superheroes.Data;
using API_WEB_Superheroes.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API_WEB_Superheroes.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual Task<bool> Actualizar(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Crear(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Eliminar(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Existe(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Existe(string nombre)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ICollection<T>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
