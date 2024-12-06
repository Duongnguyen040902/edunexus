using AutoMapper;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => new UserRoleDTO
                {
                    Id = ur.Id,
                    Role = new RoleDTO
                    {
                        Id = ur.Role.Id,
                        RoleName = ur.Role.RoleName
                    }
                }).ToList()));

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        }
    }
}