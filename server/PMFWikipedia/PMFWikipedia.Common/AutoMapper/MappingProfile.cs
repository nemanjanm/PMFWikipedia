using AutoMapper;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.Models;

namespace PMFWikipedia.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegisterInfo, User>();
        }
    }
}
