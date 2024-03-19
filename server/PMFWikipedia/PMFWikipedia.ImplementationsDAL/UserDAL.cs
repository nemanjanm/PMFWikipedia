using PMFWikipedia.ImplementationsDAL.PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.InterfacesDAL;

namespace PMFWikipedia.ImplementationsDAL
{
    public class UserDAL : BaseDAL<User>, IUserDAL
    {
        public UserDAL(PMFWikiContext context) : base(context)
        {
        }
    }
}