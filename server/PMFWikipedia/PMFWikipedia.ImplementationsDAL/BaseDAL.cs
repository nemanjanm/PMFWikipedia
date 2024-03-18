using Microsoft.EntityFrameworkCore;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.InterfacesDAL;
using System.Linq.Expressions;
using System.Reflection;

namespace PMFWikipedia.ImplementationsDAL
{
    public class BaseDAL<T> : IBaseDAL<T> where T : class
    {
        protected PMFWikiContext _context;
        protected DbSet<T> table;

        public BaseDAL(PMFWikiContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public async Task Delete(int id)
        {
            T? item = await GetById(id);
            if(item != null)
            {
                Type itemType = typeof(T);
                PropertyInfo? isDeletedProperty = itemType.GetProperty("IsDeleted");

                if(isDeletedProperty != null)
                {
                    isDeletedProperty.SetValue(item, true);
                }
                table.Update(item);
            }
        }

        public async Task<List<T>> GetAll()
        {
            return await table.ToListAsync();
        }

        public async Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter)
        {
            return await table.Where(filter).ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task Insert(T item)
        {
            await table.AddAsync(item);
        }

        public Task SaveChangesAsync(int? id = -1)
        {
            throw new NotImplementedException();
        }

        public async Task Update(T item)
        {
            table.Update(item);
        }
    }
}
