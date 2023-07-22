using AutoMapper;
using ContosoPizza.Entities;
using ContosoPizza.Models;

namespace ContosoPizza.Profiles;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<CourseDto, Course>();
        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department.Name));
    }
}