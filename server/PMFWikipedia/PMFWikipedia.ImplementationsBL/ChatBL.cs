using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsBL
{
    public class ChatBL : IChatBL
    {
        private readonly IChatDAL _chatDAL;
        private readonly IMessageDAL _messageDAL;
        public ChatBL(IChatDAL chatDAL, IMessageDAL messageDAL) 
        {
            _chatDAL = chatDAL;
            _messageDAL = messageDAL;
        }

        public async Task<ActionResultResponse<Chat>> InsertMessage(long user1Id, long user2Id, string message)
        {
            Chat chat =  await _chatDAL.GetChatId(user1Id, user2Id);
            if (chat == null)
            {
                Chat c = new Chat();
                c.User1 = user1Id;
                c.User2 = user2Id;
                await _chatDAL.Insert(c);
                await _chatDAL.SaveChangesAsync();

                Message m = new Message();
                m.Content = message;
                m.TimeStamp = DateTime.Now;
                m.IsRead = false;
                m.ChatId = c.Id;
                m.SenderId = user1Id;
                await _messageDAL.Insert(m);
                await _messageDAL.SaveChangesAsync();
            }

            else 
            {
                Message m = new Message();
                m.Content = message;
                m.TimeStamp = DateTime.Now;
                m.IsRead = false;
                m.ChatId = chat.Id;
                m.SenderId = user1Id;
                chat.DateModified = DateTime.Now;
                await _chatDAL.SaveChangesAsync();
                await _messageDAL.Insert(m);
                await _messageDAL.SaveChangesAsync();
            }
            return new ActionResultResponse<Chat>(chat, true, "");
        }
    }
}