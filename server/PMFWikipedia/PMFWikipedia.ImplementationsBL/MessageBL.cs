using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;

namespace PMFWikipedia.ImplementationsBL
{
    public class MessageBL : IMessageBL
    {
        private readonly IMessageDAL _messageDAL;
        private readonly IJWTService _jwtService;
        public MessageBL(IMessageDAL messageDAL, IJWTService jwtService) 
        {
            _messageDAL = messageDAL;
            _jwtService = jwtService;
        }

        public async Task<ActionResultResponse<long>> SetMessageAsRead(long chatId, long myId)
        {
            var messages = await _messageDAL.SetMessagesAsRead(chatId, myId);

            if (messages == null || messages.Count == 0)
                return new ActionResultResponse<long>(0, false, "Error with messages");

            foreach (var message in messages)
            {
                message.IsRead = true;
            }
            await _messageDAL.SaveChangesAsync();

            return new ActionResultResponse<long>(messages[0].SenderId, true, "");
        }
    }
}