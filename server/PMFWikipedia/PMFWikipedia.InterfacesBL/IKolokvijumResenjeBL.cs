using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IKolokvijumResenjeBL
    {
        public Task<ActionResultResponse<long>> AddKolokvijumResenje(KolokvijumResenjeModel kolokvijum);
        public Task<ActionResultResponse<bool>> DeleteResenje(long id);
    }
}