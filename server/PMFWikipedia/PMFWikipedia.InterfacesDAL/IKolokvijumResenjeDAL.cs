using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IKolokvijumResenjeDAL : IBaseDAL<KolokvijumResenje>
    {
        public Task<List<KolokvijumResenje>> GetAllWithAuthor(long kolokvijumId);
    }
}
