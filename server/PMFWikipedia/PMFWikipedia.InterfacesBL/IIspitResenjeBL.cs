using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IIspitResenjeBL
    {
        public Task<ActionResultResponse<long>> AddIspitResenje(KolokvijumResenjeModel kolokvijum);
        public Task<ActionResultResponse<bool>> DeleteResenje(long id);
    }
}