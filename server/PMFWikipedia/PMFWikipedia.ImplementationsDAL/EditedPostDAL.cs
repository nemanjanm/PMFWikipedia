using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class EditedPostDAL : BaseDAL<EditedPost>, IEditedPostDAL
    {
        public EditedPostDAL(PMFWikiContext context) : base(context) { }
    }
}