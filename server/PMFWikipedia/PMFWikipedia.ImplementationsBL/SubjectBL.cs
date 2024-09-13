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
        private readonly IProgramDAL _programDAL;
        public SubjectBL(ISubjectDAL subjectBL, IMapper mapper, IJWTService jWTService, IFavoriteSubjectDAL favoriteSubjectDAL, IProgramDAL programDAL)
        {
            _subjectDAL = subjectBL;
            _mapper = mapper;
            _jwtService = jWTService;
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _programDAL = programDAL;
        }

        public async Task<ActionResultResponse<bool>> AddProgram(string name)
        {
            Program p = new Program();
            p.Name = name;
            await _programDAL.Insert(p);
            await _programDAL.SaveChangesAsync();

            return new ActionResultResponse<bool>(true, true, "");
        }

        public async Task<ActionResultResponse<Subject>> AddSubject(SubjectModel model)
        {
            Subject s = new Subject();
            s.ProgramId = model.ProgramId;
            s.Name = model.Name;
            s.Year = model.Year;
            s.Semester = model.Semester;
            s.DateCreated = DateTime.Now;
            s.DateModified = null;
            s.LastModifiedBy = null;
            await _subjectDAL.Insert(s);
            await _subjectDAL.SaveChangesAsync(); //MORA DA SE DODA U TABELI SUBJECT ONA SRANJA...

            return new ActionResultResponse<Subject>(s, true, "");
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