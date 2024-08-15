using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IChatBL
    {
        public Task<ActionResultResponse<Chat>> InsertMessage(long user1Id, long user2Id, string message);
    }
}