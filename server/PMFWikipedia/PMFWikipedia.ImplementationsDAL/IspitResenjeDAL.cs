using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class IspitResenjeDAL : BaseDAL<IspitResenje>, IIspitResenjeDAL
    {
        public IspitResenjeDAL(PMFWikiContext context) : base(context) { }

        public async Task<List<IspitResenje>> GetAllWithAuthor(long ispitId)
        {
            return await table.Include(x => x.Author).Where(x => x.IspitId == ispitId && x.IsDeleted == false).ToListAsync();
        }
    }
}