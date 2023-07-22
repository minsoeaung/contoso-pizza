using AutoMapper;
using ContosoPizza.Entities;
using ContosoPizza.Models;

namespace ContosoPizza.Profiles;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentViewModel>();
        CreateMap<Enrollment, StudentViewModel.StudentEnrollment>();
        CreateMap<Course, StudentViewModel.EnrolledCourse>();
        CreateMap<StudentDto, Student>();
    }
}