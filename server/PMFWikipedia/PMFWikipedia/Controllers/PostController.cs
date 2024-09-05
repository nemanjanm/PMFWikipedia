using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.ImplementationsBL;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostBL _postBl;

        public PostController(IPostBL postBl)
        {
            _postBl = postBl;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(PostModel post)
        {
            return Ok(await _postBl.AddPost(post));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> DeletePost(long postId)
        {
            return Ok(await _postBl.DeletePost(postId));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPosts(long subjectId)
        {
            return Ok(await _postBl.GetAllPosts(subjectId));
        }
        [HttpGet("single")]
        public async Task<IActionResult> GetPost(long postId)
        {
            return Ok(await _postBl.GetPost(postId));
        }
    }
}
