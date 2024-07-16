using AutoMapper;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegisterInfo, User>();
            CreateMap<User, UserViewModel>();
            CreateMap<FavoriteSubject, FavoriteSubjectViewModel>()
                .ForMember(
                    dest => dest.name,
                    opt => opt.MapFrom(src => src.Subject.Name)
                );
        }
    }
}
