using PMFWikipedia.Models;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.InterfacesBL
{
    public interface IFavoriteSubjectBL
    {
        public Task<ActionResultResponse<List<FavoriteSubjectViewModel>>> GetFavoriteSubjects(long Id);
    }
}