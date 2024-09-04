using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface INotificationDAL : IBaseDAL<Notification>
    {
        public Task<List<Notification>> GetAllNotification(long id);
    }
}