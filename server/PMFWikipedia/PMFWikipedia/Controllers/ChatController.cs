using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatBL _chatBL;
        public ChatController(IChatBL chatBL)
        {
            _chatBL = chatBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats(long id)
        {
            return Ok(await _chatBL.GetAllChats(id));
        }
        [HttpGet("Messages")]
        public async Task<IActionResult> GetMessages(long id)
        {
            return Ok(await _chatBL.GetMessages(id));
        }
        [HttpGet("Unread-Messages")]
        public async Task<IActionResult> GetUnreadMessages(long id)
        {
            return Ok(await _chatBL.GetUnreadMessages(id));
        }
    }
}