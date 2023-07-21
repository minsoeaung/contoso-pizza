using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Entities;

public class Sauce
{
    public int Id { get; set; }

    [Required] [MaxLength(100)] public required string Name { get; set; }

    public bool IsVegan { get; set; }
}