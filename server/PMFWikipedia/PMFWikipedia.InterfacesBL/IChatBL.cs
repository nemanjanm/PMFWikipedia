using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.InterfacesBL
{
    public interface IChatBL
    {
        public Task<ActionResultResponse<Chat>> InsertMessage(long user1Id, long user2Id, string message);
        public Task<ActionResultResponse<List<ChatInfo>>> GetAllChats(long id);
        public Task<ActionResultResponse<List<ChatViewModel>>> GetMessages(long id);
    }
}