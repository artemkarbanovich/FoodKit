using API.DTOs.User;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddressDto, Address>()
            .ForMember(a => a.Id, opt => opt.Ignore());

        CreateMap<PersonalDataDto, AppUser>()
            .ForMember(ap => ap.RegistrationDate, opt => opt.Ignore())
            .ForMember(ap => ap.UserName, opt => opt.MapFrom(pdd => pdd.PhoneNumber.Replace("+", "")));

        CreateMap<AppUser, PersonalDataDto>();
        CreateMap<Address, AddressDto>();
    }
}