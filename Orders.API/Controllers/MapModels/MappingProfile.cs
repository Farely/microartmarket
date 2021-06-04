using AutoMapper;
using SharedData;

namespace Orders.API.Controllers.MapModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserView>();
            CreateMap<UserView, ApplicationUser>();
            CreateMap<OrderNewView, Order>();
            CreateMap<Order, OrderNewView>();
            CreateMap<ArtWork, OrderEndedView>();
            CreateMap<OrderEndedView, ArtWork>();
        }
    }
}