using System.Linq.Expressions;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IBaseDAL<T> where T : class
    {
        public Task<List<T>> GetAll();
        public Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter);
        public Task<T?> GetById(int id);
        public Task Insert (T item);
        public Task Update (T item);
        public Task Delete(int Id);
        public Task SaveChangesAsync(int? id = -1);
    }
}
