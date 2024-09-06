using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
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
                n.Id = notification.Id;
                n.PostId = notification.Post;
                n.AuthorName = notification.AuthorNavigation.FirstName + " " + notification.AuthorNavigation.LastName;
                n.SubjectName = notification.SubjectNavigation.Name;
                n.SubjectId = notification.SubjectNavigation.Id;
                n.IsRead = notification.IsRead;
                n.NotificationId = notification.NotificationId;
                n.TimeStamp = notification.DateCreated;
                list.Add(n);
            }

            return new ActionResultResponse<List<NotificationViewModel>>(list, true, "");
        }

        public async Task<ActionResultResponse<int>> GetUnreadNotification(long id)
        {
            var notts = await _notificationDAL.GetAllByFilter(x=>x.Receiver == id && x.IsRead == false && x.IsDeleted == false);
            return new ActionResultResponse<int>(notts.Count, true, "");
        }

        public async Task<ActionResultResponse<bool>> SetIsRead(long nottId)
        {
            var nott = await _notificationDAL.GetById(nottId);
            if (nott == null)
                return new ActionResultResponse<bool>(false, false, "Nottification doesnt exists");
            nott.IsRead = true;
            await _notificationDAL.SaveChangesAsync();
            return new ActionResultResponse<bool>(true, true, "Nottification set isRead");
        }
    }
}