using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class NotificationDAL : BaseDAL<Notification>, INotificationDAL 
    {
        public NotificationDAL(PMFWikiContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetAllNotification(long id)
        {
            return await table.Include(c=> c.AuthorNavigation).Include(c=>c.SubjectNavigation).Where(x=>x.Receiver == id).OrderByDescending(x=>x.DateCreated).ToListAsync();
        }
    }
}