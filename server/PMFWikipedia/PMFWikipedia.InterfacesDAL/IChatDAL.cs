using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IChatDAL : IBaseDAL<Chat>
    {
        public Task<Chat?> GetChatId(long user1Id, long user2Id);
    }
}