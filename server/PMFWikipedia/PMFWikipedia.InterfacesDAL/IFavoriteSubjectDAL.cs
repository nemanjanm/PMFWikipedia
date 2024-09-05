using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IFavoriteSubjectDAL : IBaseDAL<FavoriteSubject>
    {
        public Task<List<FavoriteSubject>> GetSubjectsByUser(long id);
        public Task<List<FavoriteSubject>> GetOnlineUsers(long id);
        public Task<FavoriteSubject> GetByFilter(RemoveFavoriteSubject fs);
        public Task<FavoriteSubject> GetByUser(long id, long subjectId);
        public Task<bool> RemoveFavoriteSubject(RemoveFavoriteSubject fs);
    }
}