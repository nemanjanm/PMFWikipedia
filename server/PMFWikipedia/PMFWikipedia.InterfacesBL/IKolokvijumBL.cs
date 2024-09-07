using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IKolokvijumBL
    {
        public Task<ActionResultResponse<bool>> AddKolokvijum(KolokvijumModel kolokvijum);
        public Task<ActionResultResponse<List<KolokvijumViewModel>>> GetAllKolokvijum(long subjectId);
    }
}