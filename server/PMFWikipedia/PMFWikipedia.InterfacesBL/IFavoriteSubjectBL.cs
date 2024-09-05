using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.InterfacesBL
{
    public interface IFavoriteSubjectBL
    {
        public Task<ActionResultResponse<List<FavoriteSubjectViewModel>>> GetFavoriteSubjects(long Id);
        public Task<ActionResultResponse<List<FavoriteSubject>>> GetOnlineUsers(long Id);
        public Task<ActionResultResponse<bool>> RemoveFavoriteSubject(RemoveFavoriteSubject fs);
        public Task<ActionResultResponse<FavoriteSubject>> AddFavoriteSubject(RemoveFavoriteSubject fs);
    }
}