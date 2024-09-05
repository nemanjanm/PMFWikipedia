using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
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
            return await table.Where(x => x.UserId == id && x.IsDeleted == false).Include(x => x.Subject).ToListAsync();
        }
        public async Task<List<FavoriteSubject>> GetOnlineUsers(long id)
        {
            return await table.Include(c =>  c.User).
                Where(x=> x.SubjectId == id && x.User.ConnectionId != "" && x.IsDeleted == false).
                Select(c => new FavoriteSubject
                {
                    Id = c.Id,
                    User = c.User
                })
                .ToListAsync();
        }

        public async Task<bool> RemoveFavoriteSubject(RemoveFavoriteSubject fs)
        {
            var rms = await table.Where(x => x.SubjectId == fs.SubjectId && x.UserId == fs.UserId && x.IsDeleted == false).FirstOrDefaultAsync();
            if (rms == null)
                return false;
            await Delete(rms.Id);
            await SaveChangesAsync();
            return true;
        }

        public async Task<FavoriteSubject> GetByFilter(RemoveFavoriteSubject fs)
        {
            return await table.Where(x => x.UserId == fs.UserId && x.SubjectId == fs.SubjectId && x.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<FavoriteSubject> GetByUser(long id, long subjectId)
        {
            return await table.Where(x => x.UserId == id && x.SubjectId == subjectId && x.IsDeleted == false).FirstOrDefaultAsync();
        }
    }
}