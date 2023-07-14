using ContosoPizza.Models;

namespace ContosoPizza.Data;

public static class DbInitializer
{
    public static void Initialize(ContosoContext context)
    {
        if (context.Pizzas.Any()
            && context.Toppings.Any()
            && context.Sauces.Any()
            && context.Students.Any()
            && context.Instructors.Any())
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

        var abercrombie = new Instructor
        {
            FirstName = "Kim",
            LastName = "Abercrombie",
            HireDate = DateTime.SpecifyKind(DateTime.Parse("1995-03-11"), DateTimeKind.Utc)
        };

        var fakhouri = new Instructor
        {
            FirstName = "Fadi",
            LastName = "Fakhouri",
            HireDate = DateTime.SpecifyKind(DateTime.Parse("2002-07-06"), DateTimeKind.Utc)
        };

        var kapoor = new Instructor
        {
            FirstName = "Candace",
            LastName = "Kapoor",
            HireDate = DateTime.SpecifyKind(DateTime.Parse("2001-01-15"), DateTimeKind.Utc)
        };

        var harui = new Instructor
        {
            FirstName = "Roger",
            LastName = "Harui",
            HireDate = DateTime.SpecifyKind(DateTime.Parse("1998-07-01"), DateTimeKind.Utc)
        };

        var zheng = new Instructor
        {
            FirstName = "Roger",
            LastName = "Zheng",
            HireDate = DateTime.SpecifyKind(DateTime.Parse("2004-02-12"), DateTimeKind.Utc)
        };

        var instructors = new[]
        {
            abercrombie,
            fakhouri,
            harui,
            kapoor,
            zheng
        };

        context.Instructors.AddRange(instructors);

        var alexander = new Student
        {
            FirstName = "Carson",
            LastName = "Alexander",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2016-09-01"), DateTimeKind.Utc)
        };

        var alonso = new Student
        {
            FirstName = "Meredith",
            LastName = "Alonso",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2018-09-01"), DateTimeKind.Utc)
        };

        var anand = new Student
        {
            FirstName = "Arturo",
            LastName = "Anand",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2019-09-01"), DateTimeKind.Utc)
        };

        var barzdukas = new Student
        {
            FirstName = "Gytis",
            LastName = "Barzdukas",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2018-09-01"), DateTimeKind.Utc)
        };

        var li = new Student
        {
            FirstName = "Yan",
            LastName = "Li",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2018-09-01"), DateTimeKind.Utc)
        };

        var justice = new Student
        {
            FirstName = "Peggy",
            LastName = "Justice",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2018-09-01"), DateTimeKind.Utc)
        };

        var norman = new Student
        {
            FirstName = "Laura",
            LastName = "Norman",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2019-09-01"), DateTimeKind.Utc)
        };

        var olivetto = new Student
        {
            FirstName = "Nino",
            LastName = "Olivetto",
            EnrollmentDate = DateTime.SpecifyKind(DateTime.Parse("2011-09-01"), DateTimeKind.Utc)
        };

        var students = new[]
        {
            alexander,
            alonso,
            anand,
            barzdukas,
            li,
            justice,
            norman,
            olivetto
        };

        context.Students.AddRange(students);

        var english = new Department
        {
            Name = "English",
            Budget = 350000,
            StartDate = DateTime.SpecifyKind(DateTime.Parse("2007-09-01"), DateTimeKind.Utc),
            Administrator = abercrombie
        };

        var mathematics = new Department
        {
            Name = "Mathematics",
            Budget = 100000,
            StartDate = DateTime.SpecifyKind(DateTime.Parse("2007-09-01"), DateTimeKind.Utc),
            Administrator = fakhouri
        };

        var engineering = new Department
        {
            Name = "Engineering",
            Budget = 350000,
            StartDate = DateTime.SpecifyKind(DateTime.Parse("2007-09-01"), DateTimeKind.Utc),
            Administrator = harui
        };

        var economics = new Department
        {
            Name = "Economics",
            Budget = 100000,
            StartDate = DateTime.SpecifyKind(DateTime.Parse("2007-09-01"), DateTimeKind.Utc),
            Administrator = kapoor
        };

        var departments = new Department[]
        {
            english,
            mathematics,
            engineering,
            economics
        };

        context.Departments.AddRange(departments);

        var chemistry = new Course
        {
            Id = 1050,
            Title = "Chemistry",
            Credits = 3,
            // Department = engineering,
            Instructors = new List<Instructor> { kapoor, harui }
        };

        var microeconomics = new Course
        {
            Id = 4022,
            Title = "Microeconomics",
            Credits = 3,
            // Department = economics,
            Instructors = new List<Instructor> { zheng }
        };

        var macroeconmics = new Course
        {
            Id = 4041,
            Title = "Macroeconomics",
            Credits = 3,
            // Department = economics,
            Instructors = new List<Instructor> { zheng }
        };

        var calculus = new Course
        {
            Id = 1045,
            Title = "Calculus",
            Credits = 4,
            // Department = mathematics,
            Instructors = new List<Instructor> { fakhouri }
        };

        var trigonometry = new Course
        {
            Id = 3141,
            Title = "Trigonometry",
            Credits = 4,
            // Department = mathematics,
            Instructors = new List<Instructor> { harui }
        };

        var composition = new Course
        {
            Id = 2021,
            Title = "Composition",
            Credits = 3,
            // Department = english,
            Instructors = new List<Instructor> { abercrombie }
        };

        var literature = new Course
        {
            Id = 2042,
            Title = "Literature",
            Credits = 4,
            // Department = english,
            Instructors = new List<Instructor> { abercrombie }
        };

        var courses = new[]
        {
            chemistry,
            microeconomics,
            macroeconmics,
            calculus,
            trigonometry,
            composition,
            literature
        };

        context.Courses.AddRange(courses);

        var enrollments = new Enrollment[]
        {
            new Enrollment
            {
                Student = alexander,
                Course = chemistry,
                Grade = Grade.A
            },
            new Enrollment
            {
                Student = alexander,
                Course = microeconomics,
                Grade = Grade.C
            },
            new Enrollment
            {
                Student = alexander,
                Course = macroeconmics,
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = alonso,
                Course = calculus,
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = alonso,
                Course = trigonometry,
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = alonso,
                Course = composition,
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = anand,
                Course = chemistry
            },
            new Enrollment
            {
                Student = anand,
                Course = microeconomics,
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = barzdukas,
                Course = chemistry,
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = li,
                Course = composition,
                Grade = Grade.B
            },
            new Enrollment
            {
                Student = justice,
                Course = literature,
                Grade = Grade.B
            }
        };

        context.Enrollments.AddRange(enrollments);

        var officeAssignments = new OfficeAssignment[]
        {
            new()
            {
                Instructor = fakhouri,
                Location = "Smith 17"
            },
            new()
            {
                Instructor = harui,
                Location = "Gowan 27"
            },
            new()
            {
                Instructor = kapoor,
                Location = "Thompson 304"
            }
        };

        context.OfficeAssignments.AddRange(officeAssignments);

        context.SaveChanges();
    }
}