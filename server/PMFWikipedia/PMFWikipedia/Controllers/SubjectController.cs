using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.ImplementationsBL;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.Models;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectBL _subjectBL;

        public SubjectController(ISubjectBL subjectBL)
        {
            _subjectBL = subjectBL;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSubjects(long programId)
        {
            return Ok(await _subjectBL.GetAllSubjects(programId));
        }
        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectModel model)
        {
            return Ok(await _subjectBL.AddSubject(model));
        }

        [HttpPost("Program")]
        public async Task<IActionResult> AddProgram(string name)
        {
            return Ok(await _subjectBL.AddProgram(name));
        }

        [HttpGet("subject")]
        public async Task<IActionResult> GetSubject(long Id)
        {
            return Ok(await _subjectBL.GetSubject(Id));
        }
    }
}