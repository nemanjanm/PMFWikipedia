using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;

namespace PMFWikipedia.ImplementationsBL
{
    public class NotificationBL : INotificationBL
    {
        private readonly INotificationDAL _notificationDAL;

        public NotificationBL(INotificationDAL notificationDAL)
        {
            _notificationDAL = notificationDAL;
        }

        public async Task<ActionResultResponse<int>> GetUnreadNotification(long id)
        {
            var notts = await _notificationDAL.GetAllByFilter(x=>x.Receiver == id && x.IsRead == false);
            return new ActionResultResponse<int>(notts.Count, true, "");
        }
    }
}