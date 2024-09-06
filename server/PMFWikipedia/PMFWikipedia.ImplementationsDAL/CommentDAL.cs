using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class CommentDAL : BaseDAL<Comment>, ICommentDAL
    {
        public CommentDAL(PMFWikiContext context) : base(context)
        {
        }
        public async Task<List<Comment>> GetAllByPostId(long postId)
        {
            return await table.Include(c=>c.User).Where(x=>x.PostId == postId && x.IsDeleted == false).OrderByDescending(x=>x.DateCreated).ToListAsync();
        }

        public async Task<List<User>> GetAllUsers(long postId)
        {
            return await table.Include(c=>c.User).Where(c=>c.PostId == postId && c.IsDeleted==false)
                .Select(c=>c.User)
                .Distinct().
                ToListAsync();
        }
    }
}