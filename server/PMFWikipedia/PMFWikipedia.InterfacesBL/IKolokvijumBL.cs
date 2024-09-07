using PMFWikipedia.Models;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.InterfacesBL
{
    public interface IKolokvijumBL
    {
        public Task<ActionResultResponse<long>> AddKolokvijum(KolokvijumModel kolokvijum);
        public Task<ActionResultResponse<List<KolokvijumViewModel>>> GetAllKolokvijum(long subjectId);
        public Task<ActionResultResponse<bool>> DeleteKolokvijum(long id);

    }
}