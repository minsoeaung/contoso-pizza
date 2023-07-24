using ContosoPizza.Entities;

namespace ContosoPizza.Services;

public interface ICourseService
{
    Task<(IEnumerable<Course>, PaginationMetaData)> GetCourses(int pageNumber, int pageSize);

    Task<Course?> GetCourse(int id);

    void AddCourse(Course course);

    void DeleteCourse(Course course);

    bool Save();

    bool DepartmentExists(int id);

    bool CourseExists(int id);
}