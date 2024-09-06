using AutoMapper;
using PMFWikipedia.ImplementationsBL.Helpers;
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
        private readonly IJWTService _jwtService;
        private readonly IFavoriteSubjectDAL _favoriteSubjectDAL;
        public SubjectBL(ISubjectDAL subjectBL, IMapper mapper, IJWTService jWTService, IFavoriteSubjectDAL favoriteSubjectDAL)
        {
            _subjectDAL = subjectBL;
            _mapper = mapper;
            _jwtService = jWTService;
            _favoriteSubjectDAL = favoriteSubjectDAL;
        }
        public async Task<ActionResultResponse<List<SubjectViewModel>>> GetAllSubjects(long programId)
        {
            List<Subject> subjects = new List<Subject>();
            subjects = await _subjectDAL.GetByProgramId(programId);
            List<SubjectViewModel> viewModel = new List<SubjectViewModel>();

            foreach (Subject subject in subjects)
            {
                SubjectViewModel svm = new SubjectViewModel();
                svm = _mapper.Map<SubjectViewModel>(subject);
                viewModel.Add(svm);
            }

            return new ActionResultResponse<List<SubjectViewModel>>(viewModel, true, "");
        }

        public async Task<ActionResultResponse<SubjectViewModel>> GetSubject(long Id)
        {
            bool allowed = true;
            var id = long.Parse(_jwtService.GetUserId());
            var favoriteSubject = await _favoriteSubjectDAL.GetByUser(id, Id);
            if (favoriteSubject == null)
                allowed = false;

            var subject = await _subjectDAL.GetById(Id);
            SubjectViewModel svm = new SubjectViewModel();
            svm = _mapper.Map<SubjectViewModel>(subject);
            svm.Allowed = allowed;

            return new ActionResultResponse<SubjectViewModel>(svm, true, "");
        }
    }
}