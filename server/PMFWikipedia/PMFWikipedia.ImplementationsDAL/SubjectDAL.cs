using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class SubjectDAL : BaseDAL<Subject>, ISubjectDAL
    {
        public SubjectDAL(PMFWikiContext context) : base(context)
        {
        }

        public async Task<List<Subject>?> GetByProgramId(long id)
        {
            return await table.Where(x=> x.ProgramId == id).ToListAsync();
        }
    }
}
