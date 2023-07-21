using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContosoPizza.Entities;

public class Topping
{
    public int Id { get; set; }

    [Required] [MaxLength(100)] public required string Name { get; set; }

    public decimal Calories { get; set; }

    [JsonIgnore] public ICollection<Pizza>? Pizzas { get; set; }
}