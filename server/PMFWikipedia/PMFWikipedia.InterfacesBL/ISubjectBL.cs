using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesBL
{
    public interface ISubjectBL
    {
        public Task<ActionResultResponse<List<SubjectViewModel>>> GetAllSubjects(long programId);
        public Task<ActionResultResponse<SubjectViewModel>> GetSubject(long Id);
        public Task<ActionResultResponse<Subject>> AddSubject(SubjectModel Id);
        public Task<ActionResultResponse<bool>> AddProgram(string name);
    }
}