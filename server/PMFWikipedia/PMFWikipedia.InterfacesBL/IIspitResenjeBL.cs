using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IIspitResenjeBL
    {
        public Task<ActionResultResponse<bool>> AddIspitResenje(KolokvijumResenjeModel kolokvijum);
    }
}