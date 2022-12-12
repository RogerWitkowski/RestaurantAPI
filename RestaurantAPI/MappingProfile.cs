using AutoMapper;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;

namespace RestaurantAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Restaurant.Models.Models.Restaurant, RestaurantDto>()
                .ForMember(m => m.Country, c => c.MapFrom(s => s.Address.Country))
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Dish, DishDto>().ReverseMap();
            CreateMap<CreateDishDto, Dish>().ReverseMap();

            CreateMap<CreateRestaurantDto, Restaurant.Models.Models.Restaurant>()
                .ForMember(m => m.Address, c => c.MapFrom(dto => new Address()
                {
                    Country = dto.Country,
                    City = dto.City,
                    Street = dto.Street,
                    PostalCode = dto.PostalCode,
                }));
        }
    }
}