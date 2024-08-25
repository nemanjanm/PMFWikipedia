using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageBL _messageBL;
        public MessageController(IMessageBL messageBL)
        {
            _messageBL = messageBL;
        }

        [HttpPost("Read")]
        public async Task<IActionResult> SetMessageAsRead(ChatIdModel chatId)
        {
            return Ok(await _messageBL.SetMessageAsRead(chatId.Id));
        }
    }
}