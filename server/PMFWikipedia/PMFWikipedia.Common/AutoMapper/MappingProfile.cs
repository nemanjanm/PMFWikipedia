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
            CreateMap<Message, ChatViewModel>();
            CreateMap<RegisterInfo, User>();
            CreateMap<PostModel, Post>();
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            CreateMap<FavoriteSubject, FavoriteSubjectViewModel>()
                .ForMember(
                    dest => dest.name,
                    opt => opt.MapFrom(src => src.Subject.Name)
                );
            CreateMap<Subject, SubjectViewModel>();
        }
    }
}
