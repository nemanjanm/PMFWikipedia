using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class NotificationDAL : BaseDAL<Notification>, INotificationDAL 
    {
        public NotificationDAL(PMFWikiContext context) : base(context)
        {
        }
    }
}