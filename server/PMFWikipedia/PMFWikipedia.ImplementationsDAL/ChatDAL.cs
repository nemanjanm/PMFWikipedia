using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

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

        public async Task<List<Chat>?> GetChatsById(long id)
        {
            return await table.Include(c => c.User1Navigation)
                .Include(c => c.User2Navigation)
                .Include(c => c.Messages)
                .Where(x => x.User1 == id || x.User2 == id)
                .Select(c => new Chat
                {
                    Id = c.Id,
                    User1 = c.User1,
                    User2 = c.User2,
                    Messages = c.Messages.Where(m => m.IsRead == false && m.SenderId != id).ToList(),
                    DateModified = c.DateModified,
                    User1Navigation = new User { Id = c.User1Navigation.Id, PhotoPath = c.User1Navigation.PhotoPath, FirstName = c.User1Navigation.FirstName, LastName = c.User1Navigation.LastName },
                    User2Navigation = new User { Id = c.User2Navigation.Id, PhotoPath = c.User2Navigation.PhotoPath, FirstName = c.User2Navigation.FirstName, LastName = c.User2Navigation.LastName }
                })
                .OrderByDescending(c=> c.DateModified)
                .ToListAsync();
        }
        public async Task<List<Chat>?> GetNumberOfUnreadMessages(long id)
        {
            return await table
                .Include(c => c.Messages)
                .Where(x => x.User1 == id || x.User2 == id)
                .Select(c => new Chat
                {
                    Messages = c.Messages.Where(m => m.IsRead == false && m.SenderId != id).ToList(),
                })
                .ToListAsync();
        }
    }
}