using AutoMapper;
using ContosoPizza.Entities;
using ContosoPizza.Models;

namespace ContosoPizza.Profiles;

public class InstructorProfile : Profile
{
    public InstructorProfile()
    {
        CreateMap<Instructor, InstructorViewModel>()
            .ForMember(dest => dest.Office,
                opt => opt.MapFrom(src => src.OfficeAssignment.Location));
    }
}