using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IPostDAL : IBaseDAL<Post>
    {
        public Task<Post> GetPostById(long id);
        public Task<List<Post>> GetAllPostsBySubject(long subjectId);
    }
}