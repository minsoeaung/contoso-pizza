using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data;

public class ContosoContext : DbContext
{
    public ContosoContext(DbContextOptions<ContosoContext> options) : base(options)
    {
    }

    public DbSet<Pizza> Pizzas => Set<Pizza>();
    public DbSet<Topping> Toppings => Set<Topping>();
    public DbSet<Sauce> Sauces => Set<Sauce>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}