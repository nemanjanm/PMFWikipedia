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

        public async Task<ActionResultResponse<object>> SetMessageAsRead(long chatId)
        {
            var userId = long.Parse(_jwtService.GetUserId());
            var messages = await _messageDAL.SetMessagesAsRead(chatId, userId);
            foreach (var message in messages)
            {
                message.IsRead = true;
            }
            await _messageDAL.SaveChangesAsync();
            if (messages != null)
                return new ActionResultResponse<object>(null, true, "");
            else
                return new ActionResultResponse<object>(null, false, "Error with messages");
        }
    }
}