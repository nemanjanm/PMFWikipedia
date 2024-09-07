using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IIspitResenjeDAL : IBaseDAL<IspitResenje>
    {
        public Task<List<IspitResenje>> GetAllWithAuthor(long subjectId);
    }
}