using Microsoft.AspNetCore.SignalR;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;
namespace PMFWikipedia.SignalR
{
    public class ChatHub : Hub
    {
        private readonly SharedDb _shared;
        private readonly IUserBL _userBL;
        private readonly IMessageBL _messageBL;
        private readonly IChatBL _chatBL;
        private readonly IPostBL _postBL;
        private readonly IFavoriteSubjectBL _favoriteSubjectBL;
        private readonly ICommentBL _commentBL;
        private readonly ICommentDAL _commentDAL;
        private readonly IUserDAL _userDAL;
        private readonly IPostDAL _postDAL;
        private readonly INotificationDAL _notificationDAL;
        public ChatHub(SharedDb shared, IUserBL userBL, IMessageBL messageBL, IChatBL chatBL, IPostBL postBL, IFavoriteSubjectBL favoriteSubjectBL, ICommentBL commentBL, ICommentDAL commentDAL, IUserDAL userDAL, IPostDAL postDAL, INotificationDAL notificationDAL)
        {
            _userBL = userBL;
            _shared = shared;
            _messageBL = messageBL;
            _chatBL = chatBL;
            _postBL = postBL;
            _favoriteSubjectBL = favoriteSubjectBL;
            _commentBL = commentBL;
            _commentDAL = commentDAL;
            _userDAL = userDAL;
            _postDAL = postDAL;
            _notificationDAL = notificationDAL;
        }

        public async Task JoinChat(UserConnection conn)
        {
            await Clients.All.SendAsync("RecieveMessage", "admin", conn.Username + " has joined");
            await Clients.User("10").SendAsync("RecieveMessage", "aa");
            
        }
        public async Task JoinSPecificChatRoom(UserConnection conn)
        {
            if(string.IsNullOrEmpty(conn.ChatRoom) || string.IsNullOrEmpty(conn.Username))
                throw new ArgumentException("Room name cannot be null or empty", nameof(conn.ChatRoom));


            await _userBL.ChangeConnectionId(conn.MyId, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);

            _shared.connections[Context.ConnectionId] = conn;

            await Clients.Group(conn.ChatRoom).SendAsync("JoinSPecificChatRoom", Context.ConnectionId , conn.MyId);
        }

        public async Task SendMessage(UserConnection conn)
        {
            ActionResultResponse<string> response = await _userBL.GetConnectionId(conn.SecondId);
            var chatviewmodel = await _chatBL.InsertMessage(conn.MyId, conn.SecondId, conn.Message);
            if (response.Data != null)
            {
                await Clients.Client(response.Data).SendAsync("ReceiveSpecificMessage", chatviewmodel);
            }
        }

        public async Task MarkAsRead(MarkAsReadInfo info)
        {
            ActionResultResponse<long> response = await _messageBL.SetMessageAsRead(info.Id, info.MyId);
            ActionResultResponse<string> response2 = await _userBL.GetConnectionId(response.Data);
            if (response.Data != null)
            {
                await Clients.Client(response2.Data).SendAsync("MarkMessagesAsRead", response.Data);
            }
        }

        public async Task SendNotfication(PostModel post)
        {
            ActionResultResponse<PostViewModel> response = await _postBL.AddPost(post);
            ActionResultResponse<List<FavoriteSubject>> favoriteSubjects = await _favoriteSubjectBL.GetOnlineUsers(post.Subject);
            foreach (var item in favoriteSubjects.Data)
            {
                if (item.UserId != post.Author)
                    await Clients.Client(item.User.ConnectionId).SendAsync("ReceiveNotification", response.Data);
            }
        }

        public async Task SendNotficationForEditPost(PostModel post)
        {
            ActionResultResponse<Post> response = await _postBL.EditPost(post);
            if (response.Data != null && response.Data.Author != post.Author)
            {
                var user = await _postDAL.GetPostById((long)post.Id);

                Notification n1 = new Notification();
                n1.Author = (long)post.Author;
                n1.Post = (long)post.Id;
                n1.Subject = (long)post.Subject;
                n1.Receiver = (long)response.Data.Author;
                n1.NotificationId = 6;
                await _notificationDAL.Insert(n1);
                await _notificationDAL.SaveChangesAsync();

                if (user != null) 
                    await Clients.Client(user.AuthorNavigation.ConnectionId).SendAsync("ReceiveCommentNotification");
            }
        }
        public async Task SendNotificationForComment(AddCommentInfo info)
        {
            ActionResultResponse<CommentViewModel> response = await _commentBL.AddComment(info);
            var me = await _userDAL.GetById(info.UserId);
            await Clients.Client(me.ConnectionId).SendAsync("ReceiveData", response.Data); //saljem sebi

            var post = await _postDAL.GetPostById(info.PostId);
            Notification n1 = new Notification();
            n1.Author = (long)info.UserId;
            n1.Post = post.Id;
            n1.Subject = (long)post.Subject;
            n1.Receiver = (long)post.Author;
            n1.NotificationId = 4;
            await _notificationDAL.Insert(n1);
            await _notificationDAL.SaveChangesAsync();

            await Clients.Client(post.AuthorNavigation.ConnectionId).SendAsync("ReceiveCommentNotification"); //poslato owneru

            var users = await _commentDAL.GetAllUsers(info.PostId);

            foreach(var user in users)
            {
                if(user.Id != info.UserId)
                {
                    Notification n = new Notification();
                    n.Author = info.UserId;
                    n.Post = post.Id;
                    n.Subject = (long)post.Subject;
                    n.Receiver = user.Id;
                    n.NotificationId = 5;
                    await _notificationDAL.Insert(n);
                    await _notificationDAL.SaveChangesAsync();

                    if (user.ConnectionId != "" && user.ConnectionId != post.AuthorNavigation.ConnectionId)
                        await Clients.Client(user.ConnectionId).SendAsync("ReceiveCommentNotification");
                }
            }
        }
        public async Task DeleteConnId(UserConnection conn)
        {
            await _userBL.ChangeConnectionId(conn.MyId, "");
        }
    }
}