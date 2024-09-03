using AutoMapper;
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
        public PostBL(IPostDAL postDAL, IMapper mapper, INotificationDAL notificationDAL, IUserDAL userDAL, ISubjectDAL subjectDAL, IFavoriteSubjectDAL favoriteSubjectDAL)
        {
            _postDAL = postDAL;
            _mapper = mapper;
            _notificationDAL = notificationDAL; 
            _userDAL = userDAL;
            _subjectDAL = subjectDAL; 
            _favoriteSubjectDAL = favoriteSubjectDAL;
        }

        public async Task<ActionResultResponse<PostViewModel>> AddPost(PostModel post)
        {
            Post p = _mapper.Map<Post>(post);
            p.LastEditedBy = post.Author;
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
                await _notificationDAL.Insert(n);
            }
            await _notificationDAL.SaveChangesAsync();
            return new ActionResultResponse<PostViewModel>(pvm, true, "");
        }

        public async Task<ActionResultResponse<List<PostViewModel>>> GetAllPosts(long subjectId)
        {
            List<Post> posts = await _postDAL.GetAllPostsBySubject(subjectId);
            List<PostViewModel> list = new List<PostViewModel>();

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
                pvm.AuthorId = (long)post.Author;

                list.Add(pvm);
            }

            return new ActionResultResponse<List<PostViewModel>>(list, true, "");
        }

        public async Task<ActionResultResponse<PostViewModel>> GetPost(long postId)
        {
            Post post = await _postDAL.GetPostById(postId);
            PostViewModel pvm = new PostViewModel();
            pvm.Title = post.Title;
            pvm.Content = post.Content;
            pvm.AuthorName = post.AuthorNavigation.FirstName + " " + post.AuthorNavigation.LastName;
            pvm.PhotoPath = post.AuthorNavigation.PhotoPath;
            pvm.PostId = postId;
            pvm.TimeStamp = post.DateCreated;
            pvm.SubjectName = post.SubjectNavigation.Name;
            pvm.AuthorId = (long)post.Author;

            return new ActionResultResponse<PostViewModel>(pvm, true, "");
        }
    }
}