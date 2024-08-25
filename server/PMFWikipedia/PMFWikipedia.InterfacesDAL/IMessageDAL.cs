using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IMessageDAL : IBaseDAL<Message>
    {
        public Task<List<Message?>> GetMessagesByChatId(long id);
        public Task<List<Message?>> SetMessagesAsRead(long chatId, long userId);
    }
}