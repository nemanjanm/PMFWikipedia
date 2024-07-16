using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class FavoriteSubjectDAL : BaseDAL<FavoriteSubject>, IFavoriteSubjectDAL
    {
        public FavoriteSubjectDAL(PMFWikiContext context) : base(context)
        {
        }

        public async Task<List<FavoriteSubject>> GetSubjectsByUser(long id)
        {
            return await table.Where(x => x.UserId == id).Include(x => x.Subject).ToListAsync();
        }
    }
}