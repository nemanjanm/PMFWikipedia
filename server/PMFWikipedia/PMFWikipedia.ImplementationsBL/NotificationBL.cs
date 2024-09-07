using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.ImplementationsBL
{
    public class NotificationBL : INotificationBL
    {
        private readonly INotificationDAL _notificationDAL;
        private readonly IKolokvijumDAL _kolokvijumDAL;
        private readonly IKolokvijumResenjeDAL _kolokvijumResenjeDAL;
        private readonly IIspitDAL _ispitDAL;
        private readonly IIspitResenjeDAL _ispitResenjeDAL;
        public NotificationBL(INotificationDAL notificationDAL, IKolokvijumDAL kolokvijumDAL, IKolokvijumResenjeDAL kolokvijumResenjeDAL, IIspitDAL ispitDAL, IIspitResenjeDAL ispitResenjeDAL)
        {
            _notificationDAL = notificationDAL;
            _kolokvijumDAL = kolokvijumDAL;
            _kolokvijumResenjeDAL = kolokvijumResenjeDAL;
            _ispitDAL = ispitDAL;
            _ispitResenjeDAL = ispitResenjeDAL;
        }

        public async Task<ActionResultResponse<List<NotificationViewModel>>> GetAllNotification(long id)
        {
            var notts = await _notificationDAL.GetAllNotification(id);
            List<NotificationViewModel> list = new List<NotificationViewModel>();
            foreach (var notification in notts) 
            {
                NotificationViewModel n = new NotificationViewModel();
                if (notification.NotificationId == 7)
                {
                    var klkresenje = await _kolokvijumResenjeDAL.GetById(notification.Post);
                    if (klkresenje != null)
                    {
                        var klk = await _kolokvijumDAL.GetById(klkresenje.KolokvijumId);
                        if (klk != null)
                            n.Title = klk.Title;
                    }
                }
                else if (notification.NotificationId == 8) 
                {
                    var ispitResenje = await _ispitResenjeDAL.GetById(notification.Post);
                    if (ispitResenje != null)
                    {
                        var ispit = await _ispitDAL.GetById(ispitResenje.IspitId);
                        if (ispit != null)
                            n.Title = ispit.Title;
                    }
                }

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