using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class PostDAL : BaseDAL<Post>, IPostDAL
    {
        public PostDAL(PMFWikiContext context) : base(context)
        {
        }
    }
}