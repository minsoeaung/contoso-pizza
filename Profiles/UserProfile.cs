using AutoMapper;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;

namespace ContosoPizza.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<IdentityUser, UserCreationResponseDto>();
    }
}