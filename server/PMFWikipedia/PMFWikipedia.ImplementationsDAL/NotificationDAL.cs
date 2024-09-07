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
            return await table.Include(c=> c.AuthorNavigation).Include(c=>c.SubjectNavigation).Where(x=>x.Receiver == id && x.IsDeleted == false).OrderByDescending(x=>x.DateCreated).ToListAsync();
        }

        public async Task<List<Notification>> GetAllKolokvijumNotification(long id)
        {
            return await table.Include(c => c.ReceiverNavigation).Where(x => x.NotificationId == 1 && x.Post == id).ToListAsync();
        }

        public async Task<List<Notification>> GetAllKolokvijumResenjeNotification(long id)
        {
            return await table.Include(c => c.ReceiverNavigation).Where(x => x.NotificationId == 7 && x.Post == id).ToListAsync();
        }
        public async Task<List<Notification>> GetAllIspitResenjeNotification(long id)
        {
            return await table.Include(c => c.ReceiverNavigation).Where(x => x.NotificationId == 8 && x.Post == id).ToListAsync();
        }

        public async Task<List<Notification>> GetAllIspitNotification(long id)
        {
            return await table.Include(c => c.ReceiverNavigation).Where(x => x.NotificationId == 2 && x.Post == id).ToListAsync();
        }
    }
}