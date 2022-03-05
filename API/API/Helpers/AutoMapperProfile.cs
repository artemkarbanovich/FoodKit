using API.DTOs;
using API.DTOs.Admin;
using API.DTOs.User;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        /*--   DTO to Entity   --*/
        CreateMap<AddressDto, Address>()
            .ForMember(a => a.Id, opt => opt.Ignore());

        CreateMap<PersonalDataDto, AppUser>()
            .ForMember(ap => ap.RegistrationDate, opt => opt.Ignore())
            .ForMember(ap => ap.UserName, opt => opt.MapFrom(pdd => pdd.PhoneNumber.Replace("+", "")));

        CreateMap<UserDishDto, UserDish>()
            .ForMember(ud => ud.Id, opt => opt.Ignore())
            .ForMember(ud => ud.Name, opt => opt.MapFrom(udd => udd.Name.ToLower()));

        CreateMap<IngredientDto, Ingredient>()
            .ForMember(i => i.Id, opt => opt.Ignore());

        CreateMap<DishAddDto, Dish>()
            .ForMember(d => d.CookingTime, opt => opt.MapFrom(dad => new TimeOnly(dad.CookingTimeHours, dad.CookingTimeMinutes)))
            .ForMember(d => d.Ingredients, opt => opt.Ignore())
            .ForMember(d => d.Images, opt => opt.Ignore());

        CreateMap<DishUpdateDto, Dish>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CookingTime, opt => opt.MapFrom(dad => new TimeOnly(dad.CookingTimeHours, dad.CookingTimeMinutes)));

        CreateMap<OrderDto, Order>()
            .ForMember(o => o.Id, opt => opt.Ignore())
            .ForMember(o => o.Evaluation, opt => opt.Ignore())
            .ForMember(o => o.OrderDishParameters, opt => opt.Ignore());

        CreateMap<OrderDishParameterDto, OrderDishParameter>()
            .ForMember(odp => odp.Id, opt => opt.Ignore());

        /*--   Entity to DTO   --*/
        CreateMap<AppUser, PersonalDataDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<UserDish, UserDishDto>();
        CreateMap<Ingredient, IngredientDto>();
        CreateMap<Image, ImageDto>();

        CreateMap<Dish, DishAdminListDto>()
            .ForMember(dgald => dgald.ImagePath, opt => opt.MapFrom(d => d.Images.FirstOrDefault().Url));

        CreateMap<Dish, DishDto>()
            .ForMember(dd => dd.CookingTimeHours, opt => opt.MapFrom(d => d.CookingTime.Hour))
            .ForMember(dd => dd.CookingTimeMinutes, opt => opt.MapFrom(d => d.CookingTime.Minute))
            .ForMember(dd => dd.Images, opt => opt.Ignore())
            .ForMember(dd => dd.Ingredients, opt => opt.Ignore());
    }
}