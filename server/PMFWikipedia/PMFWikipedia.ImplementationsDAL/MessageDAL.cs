using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class MessageDAL : BaseDAL<Message>, IMessageDAL
    {
        public MessageDAL(PMFWikiContext context) : base(context)
        {
            
        }
    }
}