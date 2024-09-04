using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.ImplementationsBL
{
    public class NotificationBL : INotificationBL
    {
        private readonly INotificationDAL _notificationDAL;

        public NotificationBL(INotificationDAL notificationDAL)
        {
            _notificationDAL = notificationDAL;
        }

        public async Task<ActionResultResponse<List<NotificationViewModel>>> GetAllNotification(long id)
        {
            var notts = await _notificationDAL.GetAllNotification(id);
            List<NotificationViewModel> list = new List<NotificationViewModel>();
            foreach (var notification in notts) 
            {
                NotificationViewModel n = new NotificationViewModel();
                n.PostId = notification.Post;
                n.AuthorName = notification.AuthorNavigation.FirstName + " " + notification.AuthorNavigation.LastName;
                n.SubjectName = notification.SubjectNavigation.Name;
                n.IsRead = notification.IsRead;
                list.Add(n);
            }

            return new ActionResultResponse<List<NotificationViewModel>>(list, true, "");
        }

        public async Task<ActionResultResponse<int>> GetUnreadNotification(long id)
        {
            var notts = await _notificationDAL.GetAllByFilter(x=>x.Receiver == id && x.IsRead == false);
            return new ActionResultResponse<int>(notts.Count, true, "");
        }
    }
}