using AutoMapper;
using SharedData;

namespace Arts.API.Controllers.MapModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserView>();
            CreateMap<UserView, ApplicationUser>();
            CreateMap<ArtWorkView, ArtWork>();
            CreateMap<ArtWork, ArtWorkView>();
        }
    }
}