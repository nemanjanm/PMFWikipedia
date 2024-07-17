using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;

        }

        [HttpPost("ChangePhoto")]
        public async Task<IActionResult> ChangePhoto(IFormFile photo)
        {
            return Ok(await _userBL.ChangePhoto(photo));
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userBL.GetUsers());
        }
    }
}
