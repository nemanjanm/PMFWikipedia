using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentBL _commentBL;
        public CommentController(ICommentBL commentBL)
        {
            _commentBL = commentBL;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentInfo info)
        {
            return Ok(await _commentBL.AddComment(info));
        }

        [HttpGet]
        public async Task<IActionResult> GetPostComments(long postId)
        {
            return Ok(await _commentBL.GetPostComments(postId));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteComment(long commentId)
        {
            return Ok(await _commentBL.DeleteComment(commentId));
        }
    }
}
