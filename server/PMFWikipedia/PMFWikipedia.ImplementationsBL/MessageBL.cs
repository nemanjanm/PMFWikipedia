using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;

namespace PMFWikipedia.ImplementationsBL
{
    public class MessageBL : IMessageBL
    {
        private readonly IMessageDAL _messageDAL;
        public MessageBL(IMessageDAL messageDAL) 
        {
            _messageDAL = messageDAL;
        }  
    }
}