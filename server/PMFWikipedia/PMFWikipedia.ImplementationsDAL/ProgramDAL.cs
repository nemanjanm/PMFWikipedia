using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class ProgramDAL : BaseDAL<Program>, IProgramDAL
    {
        public ProgramDAL(PMFWikiContext context) : base(context) { }
    }
}