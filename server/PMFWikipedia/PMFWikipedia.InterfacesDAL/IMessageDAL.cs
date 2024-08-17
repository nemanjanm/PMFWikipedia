using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IMessageDAL : IBaseDAL<Message>
    {
        public Task<List<Message?>> GetMessagesByChatId(long id);
    }
}