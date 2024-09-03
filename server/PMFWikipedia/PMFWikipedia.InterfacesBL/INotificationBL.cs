using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface INotificationBL
    {
        public Task<ActionResultResponse<int>> GetUnreadNotification(long id);
    }
}