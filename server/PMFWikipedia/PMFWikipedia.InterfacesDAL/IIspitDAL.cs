using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IIspitDAL : IBaseDAL<Ispit>
    {
        public Task<List<Ispit>> GetAllWithAuthor(long subjectId);
    }
}