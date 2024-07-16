using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}