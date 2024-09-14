using AutoMapper;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;
using System.Diagnostics.Tracing;

namespace PMFWikipedia.ImplementationsBL
{
    public class PostBL : IPostBL
    {
        private IPostDAL _postDAL;
        private IMapper _mapper;
        private INotificationDAL _notificationDAL;
        private IUserDAL _userDAL;
        private ISubjectDAL _subjectDAL;
        private IFavoriteSubjectDAL _favoriteSubjectDAL;
        private readonly ICommentDAL _commentDAL;
        private readonly IJWTService _jwtService;
        private readonly IEditedPostDAL _editedPostDAL;
        public PostBL(IPostDAL postDAL, IMapper mapper, INotificationDAL notificationDAL, IUserDAL userDAL, ISubjectDAL subjectDAL, IFavoriteSubjectDAL favoriteSubjectDAL, IJWTService jWTService, ICommentDAL commentDAL, IEditedPostDAL editedPostDAL)
        {
            _postDAL = postDAL;
            _mapper = mapper;
            _notificationDAL = notificationDAL; 
            _userDAL = userDAL;
            _subjectDAL = subjectDAL; 
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _jwtService = jWTService;
            _commentDAL = commentDAL;
            _editedPostDAL = editedPostDAL;
        }

        public async Task<ActionResultResponse<PostViewModel>> AddPost(PostModel post)
        {
            Post p = _mapper.Map<Post>(post);
            p.LastEditedBy = post.Author;
            p.DateModified = DateTime.Now;
            await _postDAL.Insert(p);
            await _postDAL.SaveChangesAsync();

            PostViewModel pvm = new PostViewModel();
            var user = await _userDAL.GetById((long)p.Author);
            pvm.AuthorName = user.FirstName + " " + user.LastName;

            var subject = await _subjectDAL.GetById((long)p.Subject);
            pvm.SubjectName = subject.Name;
            pvm.PostId = p.Id;

            var favoriteSubjects = await _favoriteSubjectDAL.GetAllByFilter(x => x.SubjectId == post.Subject && x.UserId != post.Author);

            foreach( var s in favoriteSubjects)
            {
                Notification n = new Notification();
                n.Author = post.Author;
                n.Post = p.Id;
                n.Subject = post.Subject;
                n.Receiver = s.UserId;
                n.NotificationId = 3;
                await _notificationDAL.Insert(n);
            }
            await _notificationDAL.SaveChangesAsync();
            return new ActionResultResponse<PostViewModel>(pvm, true, "");
        }

        public async Task<ActionResultResponse<List<PostViewModel>>> GetAllPosts(long subjectId)
        {
            bool allowed = true;
            var id = long.Parse(_jwtService.GetUserId());
            var favoriteSubject = await _favoriteSubjectDAL.GetByUser(id, subjectId);
            if (favoriteSubject == null) 
                allowed = false;

            List<Post> posts = await _postDAL.GetAllPostsBySubject(subjectId);
            List<PostViewModel> list = new List<PostViewModel>();

            if (posts.Count == 0)
            {
                PostViewModel pvm = new PostViewModel();
                pvm.Allowed = allowed;
                list.Add(pvm);
            }
            else
            {
                foreach (Post post in posts)
                {
                    PostViewModel pvm = new PostViewModel();
                    pvm.Title = post.Title;
                    pvm.Content = post.Content;
                    pvm.AuthorName = post.AuthorNavigation.FirstName + " " + post.AuthorNavigation.LastName;
                    pvm.PhotoPath = post.AuthorNavigation.PhotoPath;
                    pvm.PostId = post.Id;
                    pvm.TimeStamp = post.DateCreated;
                    pvm.SubjectName = post.SubjectNavigation.Name;
                    pvm.SubjectId = (long)post.Subject;
                    pvm.AuthorId = (long)post.Author;
                    pvm.Allowed = allowed;

                    List<EditPostViewModel> edits = new List<EditPostViewModel>();
                    if (post.EditedPosts.Count > 0)
                    {
                        foreach(var ep in  post.EditedPosts)
                        {
                            EditPostViewModel epvm = new EditPostViewModel();
                            epvm.Id = ep.Id;
                            epvm.Title = ep.Title;
                            epvm.Content = ep.Content;  
                            epvm.Time = ep.Time;
                            epvm.DateCreated = ep.DateCreated;
                            var editor = await _userDAL.GetById(ep.AuthorId);
                            epvm.EditorId = editor.Id;
                            epvm.EditorName = editor.FirstName + " " + editor.LastName;
                            
                            edits.Add(epvm);
                        }
                    }
                    pvm.editHistory = edits;
                    list.Add(pvm);
                }
            }
            return new ActionResultResponse<List<PostViewModel>>(list, true, "");
        }

        public async Task<ActionResultResponse<PostViewModel>> GetPost(long postId)
        {
            bool allowed = true;
            var id = long.Parse(_jwtService.GetUserId());
            

            Post post = await _postDAL.GetPostById(postId);
            if(post == null)
            {
                return new ActionResultResponse<PostViewModel>(null, false, "Something went wrong");
            }
            var favoriteSubject = await _favoriteSubjectDAL.GetByUser(id, (long)post.Subject);
            if (favoriteSubject == null)
                allowed = false;


            PostViewModel pvm = new PostViewModel();
            pvm.Title = post.Title;
            pvm.Content = post.Content;
            pvm.AuthorName = post.AuthorNavigation.FirstName + " " + post.AuthorNavigation.LastName;
            pvm.PhotoPath = post.AuthorNavigation.PhotoPath;
            pvm.PostId = postId;
            pvm.TimeStamp = post.DateCreated;
            pvm.SubjectName = post.SubjectNavigation.Name;
            pvm.SubjectId = (long)post.Subject;
            pvm.AuthorId = (long)post.Author;
            pvm.Allowed = allowed;

            List<EditPostViewModel> edits = new List<EditPostViewModel>();
            if (post.EditedPosts.Count > 0)
            {
                foreach (var ep in post.EditedPosts)
                {
                    EditPostViewModel epvm = new EditPostViewModel();
                    epvm.Id = ep.Id;
                    epvm.Title = ep.Title;
                    epvm.Content = ep.Content;
                    epvm.Time = ep.Time;
                    epvm.DateCreated = ep.DateCreated;
                    var editor = await _userDAL.GetById(ep.AuthorId);
                    epvm.EditorId = editor.Id;
                    epvm.EditorName = editor.FirstName + " " + editor.LastName;

                    edits.Add(epvm);
                }
            }
            pvm.editHistory = edits;


            return new ActionResultResponse<PostViewModel>(pvm, true, "");
        }

        public async Task<ActionResultResponse<bool>> DeletePost(long postId)
        {
            await _postDAL.Delete(postId);
            await _postDAL.SaveChangesAsync();

            var notifications = await _notificationDAL.GetAllByFilter(x => x.Post == postId);
            foreach (var notification in notifications)
            {
                await _notificationDAL.Delete(notification.Id);
                await _notificationDAL.SaveChangesAsync();
            }

            var comments = await _commentDAL.GetAllByFilter(x => x.PostId == postId);
            foreach (var comment in comments)
            {
                await _commentDAL.Delete(comment.Id);
                await _commentDAL.SaveChangesAsync();
            }

            var fs = await _postDAL.GetById(postId);
            if (fs != null && fs.IsDeleted == true)
                return new ActionResultResponse<bool>(true, true, "Successfully deleted");
            return new ActionResultResponse<bool>(false, false, "Something went wrong");
        }

        public async Task<ActionResultResponse<Post>> EditPost(PostModel post)
        {
            EditedPost ep = new EditedPost();
            var p = await _postDAL.GetById((long)post.Id);
            if (p == null)
                return new ActionResultResponse<Post>(null, false, "Something went wrong");

            ep.Title = p.Title;
            ep.Content = p.Content;
            ep.AuthorId = post.Author;
            ep.PostId = p.Id;
            ep.SubjectId = post.Subject;
            ep.Time = post.Time;

            await _editedPostDAL.Insert(ep);
            await _editedPostDAL.SaveChangesAsync();

            p.Title = post.Title;
            p.Content = post.Content;
            p.LastEditedBy = post.Author;
            p.DateModified = DateTime.Now;
            await _postDAL.Update(p);
            await _postDAL.SaveChangesAsync();

            return new ActionResultResponse<Post>(p, true, "Succesfully edited post");
        }
    }
}