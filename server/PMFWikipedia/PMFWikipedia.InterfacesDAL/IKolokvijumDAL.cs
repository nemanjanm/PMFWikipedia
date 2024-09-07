using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IKolokvijumDAL : IBaseDAL<Kolokvijum>
    {
        public Task<List<Kolokvijum>> GetAllWithAuthor(long subjectId);
    }
}