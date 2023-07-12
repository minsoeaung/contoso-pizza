using ContosoPizza.Models;

namespace ContosoPizza.Data;

public static class DbInitializer
{
    public static void Initialize(ContosoContext context)
    {
        if (context.Pizzas.Any()
            && context.Toppings.Any()
            && context.Sauces.Any()
            && context.Students.Any())
        {
            return; // DB has been seeded
        }

        var pepperoniTopping = new Topping { Name = "Pepperoni", Calories = 130 };
        var sausageTopping = new Topping { Name = "Sausage", Calories = 100 };
        var hamTopping = new Topping { Name = "Ham", Calories = 70 };
        var chickenTopping = new Topping { Name = "Chicken", Calories = 50 };
        var pineappleTopping = new Topping { Name = "Pineapple", Calories = 75 };

        var tomatoSauce = new Sauce { Name = "Tomato", IsVegan = true };
        var alfredoSauce = new Sauce { Name = "Alfredo", IsVegan = false };

        var pizzas = new[]
        {
            new Pizza
            {
                Name = "Meat Lovers",
                Sauce = tomatoSauce,
                Toppings = new List<Topping>
                {
                    pepperoniTopping,
                    sausageTopping,
                    hamTopping,
                    chickenTopping
                }
            },
            new Pizza
            {
                Name = "Hawaiian",
                Sauce = tomatoSauce,
                Toppings = new List<Topping>
                {
                    pineappleTopping,
                    hamTopping
                }
            },
            new Pizza
            {
                Name = "Alfredo Chicken",
                Sauce = alfredoSauce,
                Toppings = new List<Topping>
                {
                    chickenTopping
                }
            }
        };

        context.Pizzas.AddRange(pizzas);
        context.SaveChanges();

        var students = new[]
        {
            new Student
            {
                FirstName = "Carson", LastName = "Alexander",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2019-09-01"), DateTimeKind.Utc)
            },
            new Student
            {
                FirstName = "Meredith", LastName = "Alonso",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2017-09-01"), DateTimeKind.Utc)
            },
            new Student
            {
                FirstName = "Arturo", LastName = "Anand",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2018-09-01"), DateTimeKind.Utc)
            },
            new Student
            {
                FirstName = "Gytis", LastName = "Barzdukas",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2017-09-01"), DateTimeKind.Utc)
            },
            new Student
            {
                FirstName = "Yan", LastName = "Li",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2017-09-01"), DateTimeKind.Utc)
            },
            new Student
            {
                FirstName = "Peggy", LastName = "Justice",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2016-09-01"), DateTimeKind.Utc)
            },
            new Student
            {
                FirstName = "Laura", LastName = "Norman",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2018-09-01"), DateTimeKind.Utc)
            },
            new Student
            {
                FirstName = "Nino", LastName = "Olivetto",
                EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2019-09-01"), DateTimeKind.Utc)
            }
        };

        context.Students.AddRange(students);
        context.SaveChanges();

        var courses = new[]
        {
            new Course { Id = 1050, Title = "Chemistry", Credits = 3 },
            new Course { Id = 4022, Title = "Microeconomics", Credits = 3 },
            new Course { Id = 4041, Title = "Macroeconomics", Credits = 3 },
            new Course { Id = 1045, Title = "Calculus", Credits = 4 },
            new Course { Id = 3141, Title = "Trigonometry", Credits = 4 },
            new Course { Id = 2021, Title = "Composition", Credits = 3 },
            new Course { Id = 2042, Title = "Literature", Credits = 4 }
        };

        context.Courses.AddRange(courses);
        context.SaveChanges();

        var enrollments = new[]
        {
            new Enrollment { StudentId = 1, CourseId = 1050, Grade = Grade.A },
            new Enrollment { StudentId = 1, CourseId = 4022, Grade = Grade.C },
            new Enrollment { StudentId = 1, CourseId = 4041, Grade = Grade.B },
            new Enrollment { StudentId = 2, CourseId = 1045, Grade = Grade.B },
            new Enrollment { StudentId = 2, CourseId = 3141, Grade = Grade.F },
            new Enrollment { StudentId = 2, CourseId = 2021, Grade = Grade.F },
            new Enrollment { StudentId = 3, CourseId = 1050 },
            new Enrollment { StudentId = 4, CourseId = 1050 },
            new Enrollment { StudentId = 4, CourseId = 4022, Grade = Grade.F },
            new Enrollment { StudentId = 5, CourseId = 4041, Grade = Grade.C },
            new Enrollment { StudentId = 6, CourseId = 1045 },
            new Enrollment { StudentId = 7, CourseId = 3141, Grade = Grade.A },
        };

        context.Enrollments.AddRange(enrollments);
        context.SaveChanges();
    }
}