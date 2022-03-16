namespace API_WEB_Superheroes.Repository.IRepositories
{

    public interface IGenericRepository<T> where T : class
    {
        Task<ICollection<T>> ObtenerTodos();
        Task<T> ObtenerPorId(int id);
        Task<bool> Crear(T entity);
        Task<bool> Actualizar(T entity);
        Task<bool> Eliminar(T entity);
        Task<bool> Existe(int id);
        Task<bool> Existe(string nombre);
    }
}
