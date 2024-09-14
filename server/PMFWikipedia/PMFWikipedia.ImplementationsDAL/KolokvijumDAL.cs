using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class KolokvijumDAL : BaseDAL<Kolokvijum>, IKolokvijumDAL
    {
        public KolokvijumDAL(PMFWikiContext context) : base(context)
        {
        }

        public async Task<List<Kolokvijum>> GetAllWithAuthor(long subjectId)
        {
            return await table.Include(x => x.Author).Where(x => x.SubjectId == subjectId && x.IsDeleted == false).OrderByDescending(x=>x.Year).ToListAsync();
        }

        public async Task<Kolokvijum> GetByTitle(string title)
        {
            return await table.Where(x => x.Title == title && x.IsDeleted == false).FirstOrDefaultAsync();
        }
    }
}