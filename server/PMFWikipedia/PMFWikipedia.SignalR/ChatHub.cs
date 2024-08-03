using Microsoft.AspNetCore.SignalR;
using PMFWikipedia.Models;
namespace PMFWikipedia.SignalR
{
    public class ChatHub : Hub
    {
        private readonly SharedDb _shared;

        public ChatHub(SharedDb shared)
        {
            _shared = shared;
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

            
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);

            _shared.connections[Context.ConnectionId] = conn;

            await Clients.Group(conn.ChatRoom).SendAsync("JoinSPecificChatRoom", Context.ConnectionId , conn.MyId);
        }

        public async Task SendMessage(UserConnection conn)
        {
            //DOBRO SE SALJE PORUKA, CONSOLE LOGUJEM JE, POTREBNO JE SACUVATI JE U BAZI I VIDETI KAKO DALJE
            await Clients.Client(conn.SecondId).SendAsync("ReceiveSpecificMessage", conn.SecondId, conn.Message);
        }
    }
}