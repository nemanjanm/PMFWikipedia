using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class ChatDAL : BaseDAL<Chat>, IChatDAL
    {
        public ChatDAL(PMFWikiContext context) : base(context) 
        {
        }

        public async Task<Chat?> GetChatId(long user1Id, long user2Id)
        {
            return await table.Where(x =>( x.User1 == user1Id && x.User2 == user2Id) || (x.User1 == user2Id && x.User2 == user1Id)).FirstOrDefaultAsync();
        }
    }
}