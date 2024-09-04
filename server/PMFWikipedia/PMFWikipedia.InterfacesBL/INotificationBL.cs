using PMFWikipedia.Models;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.InterfacesBL
{
    public interface INotificationBL
    {
        public Task<ActionResultResponse<int>> GetUnreadNotification(long id);
        public Task<ActionResultResponse<List<NotificationViewModel>>> GetAllNotification(long id);
    }
}