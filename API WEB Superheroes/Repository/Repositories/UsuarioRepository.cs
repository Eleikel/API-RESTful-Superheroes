using API_WEB_Superheroes.Data;
using API_WEB_Superheroes.Models;
using API_WEB_Superheroes.Repository.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API_WEB_Superheroes.Repository.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {

        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExisteUsuario(string user)
        {
            if (await dbSet.AnyAsync(x => x.UsuarioA == user))
            {
                return true;
            }
            return false;
        }

        public async Task<Usuario> ObtenerUsuario(int UserId)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await dbSet.FirstOrDefaultAsync(x => x.Id == UserId);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public async Task<ICollection<Usuario>> ObtenerUsuarios()
        {
            return await dbSet.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Usuario> Loguearse(string user, string password)
        {

            var getUser = dbSet.FirstOrDefault(x => x.UsuarioA == user);

            if (getUser == null)
            {
                return null;
            }

            if (!await VerifyPasswordHash(password, getUser.PasswordHash, getUser.PasswordSalt))
            {
                return null;
            }
            return getUser;
        }


        public Task<Usuario> Registrar(Usuario user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            dbSet.Add(user);
            return Task.FromResult(user); ;
        }




        private Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i])
                    {
                        return Task.FromResult(false);
                    }
                }
            }
            return Task.FromResult(true);
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        Task<ICollection<UsuarioRepository>> IGenericRepository<UsuarioRepository>.ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        Task<UsuarioRepository> IGenericRepository<UsuarioRepository>.ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Crear(UsuarioRepository entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Actualizar(UsuarioRepository entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(UsuarioRepository entity)
        {
            throw new NotImplementedException();
        }
    }
}
