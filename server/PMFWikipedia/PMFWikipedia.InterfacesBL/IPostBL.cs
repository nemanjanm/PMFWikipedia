using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IPostBL
    {
        public Task<ActionResultResponse<PostModel>> AddPost(PostModel post);
    }
}