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
            return await table.Include(x => x.Author).Where(x => x.SubjectId == subjectId && x.IsDeleted == false).ToListAsync();
        }
    }
}