using Microsoft.AspNetCore.SignalR;
using PMFWikipedia.InterfacesBL;
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
        public ChatHub(SharedDb shared, IUserBL userBL, IMessageBL messageBL, IChatBL chatBL, IPostBL postBL, IFavoriteSubjectBL favoriteSubjectBL)
        {
            _userBL = userBL;
            _shared = shared;
            _messageBL = messageBL;
            _chatBL = chatBL;
            _postBL = postBL;
            _favoriteSubjectBL = favoriteSubjectBL; 
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

        public async Task DeleteConnId(UserConnection conn)
        {
            await _userBL.ChangeConnectionId(conn.MyId, "");
        }
    }
}