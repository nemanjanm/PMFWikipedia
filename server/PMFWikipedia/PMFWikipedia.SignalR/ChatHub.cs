using Microsoft.AspNetCore.SignalR;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
namespace PMFWikipedia.SignalR
{
    public class ChatHub : Hub
    {
        private readonly SharedDb _shared;
        private readonly IUserBL _userBL;
        private readonly IMessageBL _messageBL;
        private readonly IChatBL _chatBL;
        public ChatHub(SharedDb shared, IUserBL userBL, IMessageBL messageBL, IChatBL chatBL)
        {
            _userBL = userBL;
            _shared = shared;
            _messageBL = messageBL;
            _chatBL = chatBL;
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
            await _chatBL.InsertMessage(conn.MyId, conn.SecondId, conn.Message);
            if (response.Data != null)
            {
                await Clients.Client(response.Data).SendAsync("ReceiveSpecificMessage", conn.SecondId, conn.Message);
            }
        }

        public async Task DeleteConnId(UserConnection conn)
        {
            await _userBL.ChangeConnectionId(conn.MyId, "");
        }
    }
}