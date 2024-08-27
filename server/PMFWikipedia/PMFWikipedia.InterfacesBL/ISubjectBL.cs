using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface ISubjectBL
    {
        public Task<ActionResultResponse<List<SubjectViewModel>>> GetAllSubjects(long programId);
    }
}