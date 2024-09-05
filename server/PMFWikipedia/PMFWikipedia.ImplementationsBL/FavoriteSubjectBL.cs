using AutoMapper;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.ImplementationsBL
{
    public class FavoriteSubjectBL : IFavoriteSubjectBL
    {
        private readonly IFavoriteSubjectDAL _favoriteSubjectDAL;
        private readonly IMapper _mapper;

        public FavoriteSubjectBL(IFavoriteSubjectDAL favoriteSubjectDAL, IMapper mapper)
        {
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _mapper = mapper;
        }

        public async Task<ActionResultResponse<FavoriteSubject>> AddFavoriteSubject(RemoveFavoriteSubject fs)
        {
            var checkFs = await _favoriteSubjectDAL.GetByFilter(fs);
            if (checkFs == null) 
            {
                FavoriteSubject f = new FavoriteSubject();
                f.SubjectId = fs.SubjectId;
                f.UserId = fs.UserId;
                await _favoriteSubjectDAL.Insert(f);
                await _favoriteSubjectDAL.SaveChangesAsync();

                return new ActionResultResponse<FavoriteSubject>(f, true, "Successfully added favorite subject");
            }
            else
                return new ActionResultResponse<FavoriteSubject>(null, false, "Favorite subject already exists");

        }

        public async Task<ActionResultResponse<List<FavoriteSubjectViewModel>>> GetFavoriteSubjects(long Id)
        {
            List<FavoriteSubject> favorites = new List<FavoriteSubject>();

            List<FavoriteSubjectViewModel> favoriteSubjects = new List<FavoriteSubjectViewModel>();

            favorites = await _favoriteSubjectDAL.GetSubjectsByUser(Id);
            if(favorites.Count < 1)
                return new ActionResultResponse<List<FavoriteSubjectViewModel>>(null, false, "Empty list");

            foreach (FavoriteSubject s in favorites)
            {
                FavoriteSubjectViewModel f = new FavoriteSubjectViewModel();
                f = _mapper.Map<FavoriteSubjectViewModel>(s);
                favoriteSubjects.Add(f);
            }

            return new ActionResultResponse<List<FavoriteSubjectViewModel>>(favoriteSubjects, true, "");
        }

        public async Task<ActionResultResponse<List<FavoriteSubject>>> GetOnlineUsers(long Id)
        {
            List<FavoriteSubject> favoriteSubjects = await _favoriteSubjectDAL.GetOnlineUsers(Id);
            return new ActionResultResponse<List<FavoriteSubject>>(favoriteSubjects, true, "");

        }

        public async Task<ActionResultResponse<bool>> RemoveFavoriteSubject(RemoveFavoriteSubject fs)
        {
            var response = await _favoriteSubjectDAL.RemoveFavoriteSubject(fs);
            return new ActionResultResponse<bool>(response, true, "");
        }
    }
}