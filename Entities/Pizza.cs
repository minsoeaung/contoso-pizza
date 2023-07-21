using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Entities;

public class Pizza
{
    public int Id { get; set; }

    [Required] [MaxLength(100)] public required string Name { get; set; }

    public Sauce? Sauce { get; set; }

    public ICollection<Topping>? Toppings { get; set; }
}