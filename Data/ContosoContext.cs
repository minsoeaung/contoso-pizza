using ContosoPizza.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data;

public class ContosoContext : IdentityUserContext<IdentityUser>
{
    public ContosoContext(DbContextOptions<ContosoContext> options) : base(options)
    {
    }

    public DbSet<Pizza> Pizzas => Set<Pizza>();
    public DbSet<Topping> Toppings => Set<Topping>();
    public DbSet<Sauce> Sauces => Set<Sauce>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<OfficeAssignment> OfficeAssignments => Set<OfficeAssignment>();
    public DbSet<UserApiKey> UserApiKeys => Set<UserApiKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call IdentityUserContext's own OnModelCreating
        base.OnModelCreating(modelBuilder);
    }
}