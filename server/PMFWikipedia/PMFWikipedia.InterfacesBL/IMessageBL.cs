using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IMessageBL
    {
        public Task<ActionResultResponse<object>> SetMessageAsRead(long chatId);
    }
}