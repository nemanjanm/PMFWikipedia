using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;

        }

        [HttpPost("ChangePhoto")]
        public async Task<IActionResult> ChangePhoto(long id, IFormFile photo)
        {
            return Ok(await _userBL.ChangePhoto(id, photo));
        }
    }
}
