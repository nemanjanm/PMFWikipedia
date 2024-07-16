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

        public async Task<ActionResultResponse<List<FavoriteSubjectViewModel>>> GetFavoriteSubjects(long Id)
        {
            List<FavoriteSubject> favorites = new List<FavoriteSubject>();

            List<FavoriteSubjectViewModel> favoriteSubjects = new List<FavoriteSubjectViewModel>();

            favorites = await _favoriteSubjectDAL.GetSubjectsByUser(Id);
            if(favorites.Count < 1)
                return new ActionResultResponse<List<FavoriteSubjectViewModel>>(null, false, "Empty list");

            foreach (FavoriteSubject s in favorites)
            {
                Console.WriteLine(s.Subject);
                FavoriteSubjectViewModel f = new FavoriteSubjectViewModel();
                f = _mapper.Map<FavoriteSubjectViewModel>(s);
                favoriteSubjects.Add(f);
            }

            return new ActionResultResponse<List<FavoriteSubjectViewModel>>(favoriteSubjects, true, "");
        }
    }
}