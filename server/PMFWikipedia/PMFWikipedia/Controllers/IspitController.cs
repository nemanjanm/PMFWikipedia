using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.ImplementationsBL;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class IspitController : ControllerBase
    {
        private readonly IIspitBL _ispitBL;
        private readonly IIspitResenjeBL _ispitResenjeBL;

        public IspitController(IIspitBL ispitBL, IIspitResenjeBL ispitResenjeBL)
        {
            _ispitBL = ispitBL;
            _ispitResenjeBL = ispitResenjeBL;
        }

        [HttpPost]
        public async Task<IActionResult> AddIspit([FromForm] KolokvijumModel ispit)
        {
            return Ok(await _ispitBL.AddIspit(ispit));
        }
        [HttpPost("resenje")]
        public async Task<IActionResult> AddResenje([FromForm] KolokvijumResenjeModel resenje)
        {
            return Ok(await _ispitResenjeBL.AddIspitResenje(resenje));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllKolokvijum(long subjectId)
        {
            return Ok(await _ispitBL.GetAllIspit(subjectId));
        }
    }
}