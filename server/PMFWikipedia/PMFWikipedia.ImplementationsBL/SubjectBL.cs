using AutoMapper;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.ImplementationsBL
{
    public class SubjectBL : ISubjectBL
    {
        private readonly ISubjectDAL _subjectDAL;
        private readonly IMapper _mapper;
        public SubjectBL(ISubjectDAL subjectBL, IMapper mapper)
        {
            _subjectDAL = subjectBL;
            _mapper = mapper;
        }
        public async Task<ActionResultResponse<List<SubjectViewModel>>> GetAllSubjects()
        {
            List<Subject> subjects = new List<Subject>();
            subjects = await _subjectDAL.GetAll();
            List<SubjectViewModel> viewModel = new List<SubjectViewModel>();

            foreach (Subject subject in subjects)
            {
                SubjectViewModel svm = new SubjectViewModel();
                svm = _mapper.Map<SubjectViewModel>(subject);
                viewModel.Add(svm);
            }

            return new ActionResultResponse<List<SubjectViewModel>>(viewModel, true, "");
        }
    }
}