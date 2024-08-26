using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IMessageBL
    {
        public Task<ActionResultResponse<long>> SetMessageAsRead(long chatId, long myId);
    }
}