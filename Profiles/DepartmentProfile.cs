using AutoMapper;
using ContosoPizza.Entities;
using ContosoPizza.Models;

namespace ContosoPizza.Profiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>()
            // Put Administrator as a string from Administrator.FullName property
            .ForMember(dest => dest.Administrator,
                opt => opt.MapFrom(src => src.Administrator.FullName));
    }
}