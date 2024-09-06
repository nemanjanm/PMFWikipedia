using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.ImplementationsBL
{
    public class CommentBL : ICommentBL
    {
        private readonly IJWTService _jWTService;
        private readonly ICommentDAL _commentDAL;
        private readonly IUserDAL _userDAL;
        private readonly IPostDAL _postDAL;

        public CommentBL(IJWTService jWTService, ICommentDAL commentDAL, IUserDAL userDAL, IPostDAL postDAL)
        {
            _jWTService = jWTService;
            _commentDAL = commentDAL;
            _userDAL = userDAL;
            _postDAL = postDAL; 
        }

        public async Task<ActionResultResponse<CommentViewModel>> AddComment(AddCommentInfo info)
        {
            var user = await _userDAL.GetById(info.UserId);
            if (user == null)
            {
                return new ActionResultResponse<CommentViewModel>(null, false, "Something went wrong");
            }
            var post = await _postDAL.GetById(info.PostId);
            if (post == null)
            {
                return new ActionResultResponse<CommentViewModel>(null, false, "Something went wrong");
            }

            Comment c = new Comment();
            c.UserId = user.Id;
            c.PostId = info.PostId;
            c.Content = info.Content;

            await _commentDAL.Insert(c);
            await _commentDAL.SaveChangesAsync();

            CommentViewModel cvm = new CommentViewModel();
            cvm.Id = c.Id;
            cvm.Content = c.Content;
            cvm.TimeStamp = c.DateCreated;
            cvm.PhotoPath = user.PhotoPath;
            cvm.UserName = user.FirstName + " " + user.LastName;
            cvm.UserId = user.Id;
            
            return new ActionResultResponse<CommentViewModel>(cvm, true, "Succesfully Added Comment");
        }

        public async Task<ActionResultResponse<bool>> DeleteComment(long commentId)
        {
            var id = long.Parse(_jWTService.GetUserId());
            var comment = await _commentDAL.GetById(commentId);
            if(comment!=null && comment.UserId != id)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            await _commentDAL.Delete(commentId);
            await _commentDAL.SaveChangesAsync();

            var c = await _commentDAL.GetById(commentId);
            if (c != null && c.IsDeleted == true)
                return new ActionResultResponse<bool>(true, true, "Successfully deleted");
            return new ActionResultResponse<bool>(false, false, "Something went wrong");
        }

        public async Task<ActionResultResponse<List<CommentViewModel>>> GetPostComments(long postId)
        {
            var comments = await _commentDAL.GetAllByPostId(postId);
            List<CommentViewModel> list = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                CommentViewModel cvm = new CommentViewModel();
                cvm.Id = comment.Id;
                cvm.UserId = comment.UserId;
                cvm.UserName = comment.User.FirstName + " " + comment.User.LastName;
                cvm.PhotoPath = comment.User.PhotoPath;
                cvm.Content = comment.Content;
                cvm.TimeStamp = comment.DateCreated;

                list.Add(cvm);
            }
            return new ActionResultResponse<List<CommentViewModel>>(list, true, "");
        }
    }
}