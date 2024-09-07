using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IIspitBL
    {
        public Task<ActionResultResponse<long>> AddIspit(KolokvijumModel ispit);
        public Task<ActionResultResponse<List<KolokvijumViewModel>>> GetAllIspit(long subjectId);
        public Task<ActionResultResponse<bool>> DeleteIspit(long id);
    }
}