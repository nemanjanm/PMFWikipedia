﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> ChangePhoto([FromForm]IFormFile photo)
        {
            return Ok(await _userBL.ChangePhoto(photo));
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetUsers(long programId)
        {
            return Ok(await _userBL.GetUsers(programId));
        }
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(long id)
        {
            return Ok(await _userBL.GetUser(id));
        }
    }
}
