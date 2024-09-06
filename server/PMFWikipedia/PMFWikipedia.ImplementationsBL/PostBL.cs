using AutoMapper;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

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
        private readonly IJWTService _jwtService;
        public PostBL(IPostDAL postDAL, IMapper mapper, INotificationDAL notificationDAL, IUserDAL userDAL, ISubjectDAL subjectDAL, IFavoriteSubjectDAL favoriteSubjectDAL, IJWTService jWTService)
        {
            _postDAL = postDAL;
            _mapper = mapper;
            _notificationDAL = notificationDAL; 
            _userDAL = userDAL;
            _subjectDAL = subjectDAL; 
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _jwtService = jWTService;
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
                    pvm.TimeEdited = post.DateModified;
                    pvm.EditorName = post.LastEditedByNavigation.FirstName + " " + post.LastEditedByNavigation.LastName;
                    pvm.EditorId = post.LastEditedByNavigation.Id;
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
            pvm.TimeEdited = post.DateModified;
            pvm.EditorName = post.LastEditedByNavigation.FirstName + " " + post.LastEditedByNavigation.LastName;
            pvm.EditorId = post.LastEditedByNavigation.Id;

            return new ActionResultResponse<PostViewModel>(pvm, true, "");
        }

        public async Task<ActionResultResponse<bool>> DeletePost(long postId)
        {
            await _postDAL.Delete(postId);
            await _postDAL.SaveChangesAsync();

            var fs = await _postDAL.GetById(postId);
            if (fs != null && fs.IsDeleted == true)
                return new ActionResultResponse<bool>(true, true, "Successfully deleted");
            return new ActionResultResponse<bool>(false, false, "Something went wrong");
        }

        public async Task<ActionResultResponse<PostModel>> EditPost(PostModel post)
        {
            var p = await _postDAL.GetById((long)post.Id);
            if (p == null)
                return new ActionResultResponse<PostModel>(null, false, "Something went wrong");

            p.Title = post.Title;
            p.Content = post.Content;
            p.LastEditedBy = post.Author;
            p.DateModified = DateTime.Now;
            await _postDAL.Update(p);
            await _postDAL.SaveChangesAsync();

            return new ActionResultResponse<PostModel>(post, true, "Something went wrong");
        }
    }
}