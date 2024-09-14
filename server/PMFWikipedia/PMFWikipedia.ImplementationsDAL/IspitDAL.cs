using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class IspitDAL : BaseDAL<Ispit>, IIspitDAL
    {
        public IspitDAL(PMFWikiContext context) : base(context)
        {
        }
        public async Task<List<Ispit>> GetAllWithAuthor(long subjectId)
        {
            return await table.Include(x => x.Author).Where(x => x.SubjectId == subjectId && x.IsDeleted == false).OrderByDescending(x=>x.Year).ToListAsync();
        }

        public async Task<Ispit> GetByTitle(string title)
        {
            return await table.Where(x => x.Title == title && x.IsDeleted == false).FirstOrDefaultAsync();
        }
    }
}