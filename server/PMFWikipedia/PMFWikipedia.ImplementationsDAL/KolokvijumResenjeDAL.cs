using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class KolokvijumResenjeDAL : BaseDAL<KolokvijumResenje>, IKolokvijumResenjeDAL
    {
        public KolokvijumResenjeDAL(PMFWikiContext context) : base(context) 
        {
        }

        public async Task<List<KolokvijumResenje>> GetAllWithAuthor(long kolokvijumId)
        {
            return await table.Include(x=>x.Author).Where(x=>x.KolokvijumId == kolokvijumId && x.IsDeleted == false).ToListAsync();
        }
    }
}
