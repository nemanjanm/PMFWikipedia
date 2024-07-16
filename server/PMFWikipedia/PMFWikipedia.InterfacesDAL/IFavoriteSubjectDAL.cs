using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IFavoriteSubjectDAL : IBaseDAL<FavoriteSubject>
    {
        public Task<List<FavoriteSubject>> GetSubjectsByUser(long id);
    }
}
