using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FavoriteSubject : ControllerBase
    {
        private readonly IFavoriteSubjectBL _favoriteSubjectBL;

        public FavoriteSubject(IFavoriteSubjectBL favoriteSubject)
        {
            _favoriteSubjectBL = favoriteSubject;
        }

        [HttpGet ("GetFavoriteSubjects")]
        public async Task<IActionResult> GetFavoriteSubjects(long id)
        {
            return Ok(await _favoriteSubjectBL.GetFavoriteSubjects(id));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteSubject(RemoveFavoriteSubject fs)
        {
            return Ok(await _favoriteSubjectBL.RemoveFavoriteSubject(fs));
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddFavoriteSubject(RemoveFavoriteSubject fs)
        {
            return Ok(await _favoriteSubjectBL.AddFavoriteSubject(fs));
        }
    }
}