using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface INotificationDAL : IBaseDAL<Notification>
    {
        public Task<List<Notification>> GetAllNotification(long id);
        public Task<List<Notification>> GetAllKolokvijumNotification(long id);
        public Task<List<Notification>> GetAllIspitNotification(long id);
        public Task<List<Notification>> GetAllKolokvijumResenjeNotification(long id);
        public Task<List<Notification>> GetAllIspitResenjeNotification(long id);
    }
}