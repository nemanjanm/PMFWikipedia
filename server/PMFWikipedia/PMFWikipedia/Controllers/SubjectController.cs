using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.ImplementationsBL;
using PMFWikipedia.InterfacesBL;

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

        [HttpGet("subject")]
        public async Task<IActionResult> GetAllSubject(long Id)
        {
            return Ok(await _subjectBL.GetSubject(Id));
        }
    }
}