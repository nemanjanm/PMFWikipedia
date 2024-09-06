using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface ICommentDAL : IBaseDAL<Comment>
    {
        public Task<List<Comment>> GetAllByPostId(long postId);
        public Task<List<User>> GetAllUsers(long postId);
    }
}