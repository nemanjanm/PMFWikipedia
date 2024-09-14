using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.InterfacesBL
{
    public interface ICommentBL
    {
        public Task<ActionResultResponse<CommentViewModel>> AddComment(AddCommentInfo info);
        public Task<ActionResultResponse<List<CommentViewModel>>> GetPostComments(long postId);
        public Task<ActionResultResponse<bool>> DeleteComment(DeleteCommentModel model);
    }
}