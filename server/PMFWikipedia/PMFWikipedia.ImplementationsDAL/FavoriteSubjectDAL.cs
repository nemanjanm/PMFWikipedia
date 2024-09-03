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
        public async Task<List<FavoriteSubject>> GetOnlineUsers(long id)
        {
            return await table.Include(c =>  c.User).
                Where(x=> x.SubjectId == id && x.User.ConnectionId != "").
                Select(c => new FavoriteSubject
                {
                    Id = c.Id,
                    User = c.User
                })
                .ToListAsync();
        }
    }
}