using Minimarket.Entity;
using AutoMapper;
using Minimarket.DTO.User;

namespace Minimarket.DAL.Automapper;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<UserProfile, UserProfileDTO>();
        CreateMap<UserProfileDTO, UserProfile>();

        CreateMap<ResponseUserDTO, UserProfile>()
            .ForMember(d => d.Id, opt => opt.Ignore());

        CreateMap<UserProfile, ResponseUserDTO>();
    }
    
}