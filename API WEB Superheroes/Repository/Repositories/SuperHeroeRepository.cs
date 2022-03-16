using API_WEB_Superheroes.Data;
using API_WEB_Superheroes.Models;
using API_WEB_Superheroes.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API_WEB_Superheroes.Repository.Repositories
{
    public class SuperHeroeRepository : GenericRepository<SuperHeroe>, ISuperHeroeRepository
    {
        public SuperHeroeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Task<bool> Actualizar(SuperHeroe entity)
        {
            dbSet.Update(entity);
            return Task.FromResult(true);
        }

        public override async Task<bool> Crear(SuperHeroe entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public override Task<bool> Eliminar(SuperHeroe entity)
        {
            dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public override async Task<bool> Existe(int id)
        {
            var exist = await dbSet.AnyAsync(x => x.Id == id);
            return exist;
        }

        public override async Task<bool> Existe(string nombreSuperHeroe)
        {
            var exist = await dbSet.AnyAsync(x => x.NombreSuperHeroe.ToLower().Trim() == nombreSuperHeroe.ToLower().Trim());
            return exist;
        }

        public override async Task<SuperHeroe> ObtenerPorId(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public override async Task<ICollection<SuperHeroe>> ObtenerTodos()
        {
            return await dbSet.OrderBy(a => a.Id).ToListAsync();
        }

        public  async Task<IEnumerable<SuperHeroe>> BuscarSuperHeroe(string name)
        {
            IQueryable<SuperHeroe> query = dbSet;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.NombreSuperHeroe.Contains(name));
            }

            return await query.ToListAsync();
        }
    }
}
