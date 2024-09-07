using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IKolokvijumResenjeBL
    {
        public Task<ActionResultResponse<bool>> AddKolokvijumResenje(KolokvijumResenjeModel kolokvijum);
    }
}