using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IPostBL
    {
        public Task<ActionResultResponse<PostViewModel>> AddPost(PostModel post); //NotificationviewModel?
        public Task<ActionResultResponse<PostViewModel>> GetPost(long postId);
        public Task<ActionResultResponse<List<PostViewModel>>> GetAllPosts(long subjectId);
    }
}