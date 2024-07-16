using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class SubjectDAL : BaseDAL<Subject>, ISubjectDAL
    {
        public SubjectDAL(PMFWikiContext context) : base(context)
        {
        }
    }
}
