using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public AuthController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterInfo registerInfo)
        {
            return Ok(await _userBL.Register(registerInfo));
        }

        [HttpPost("ConfirmRegistration")]
        public async Task<IActionResult> Register(string registrationToken)
        {
            return Ok(await _userBL.ValidateUser(registrationToken));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginInfo loginInfo)
        {
            return Ok(await _userBL.Login(loginInfo));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
        {
            return Ok(await _userBL.CreateResetToken(email));
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ResetPasswordInfo info)
        {
            return Ok(await _userBL.ResetPassword(info));
        }
    }
}