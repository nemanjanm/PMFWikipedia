using PMFWikipedia.ImplementationsDAL.PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.InterfacesDAL;

namespace PMFWikipedia.ImplementationsDAL
{
    public class SubjectDAL : BaseDAL<Subject>, ISubjectDAL
    {
        public SubjectDAL(PMFWikiContext context) : base(context)
        {
        }
    }
}
