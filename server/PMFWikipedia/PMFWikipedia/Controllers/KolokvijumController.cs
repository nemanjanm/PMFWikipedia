using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KolokvijumController : ControllerBase
    {
        private readonly IKolokvijumBL _kolokvijumBL;
        private readonly IKolokvijumResenjeBL _kolokvijumResenjeBL;

        public KolokvijumController(IKolokvijumBL kolokvijumBL, IKolokvijumResenjeBL kolokvijumResenjeBL)
        {
            _kolokvijumBL = kolokvijumBL;
            _kolokvijumResenjeBL = kolokvijumResenjeBL;
        }

        [HttpPost]
        public async Task<IActionResult> AddKolokvijum([FromForm]KolokvijumModel kolokvijum)
        {
            return Ok(await _kolokvijumBL.AddKolokvijum(kolokvijum));
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteKolokvijum(long id)
        {
            return Ok(await _kolokvijumBL.DeleteKolokvijum(id));
        }
        [HttpPost("delete/resenje")]
        public async Task<IActionResult> DeleteResenje(long id)
        {
            return Ok(await _kolokvijumResenjeBL.DeleteResenje(id));
        }
        [HttpPost("resenje")]
        public async Task<IActionResult> AddResenje([FromForm] KolokvijumResenjeModel resenje)
        {
            return Ok(await _kolokvijumResenjeBL.AddKolokvijumResenje(resenje));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllKolokvijum(long subjectId)
        {
            return Ok(await _kolokvijumBL.GetAllKolokvijum(subjectId));
        }
    }
}