using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Entities;

[Index(nameof(Value), IsUnique = true)]
public class UserApiKey
{
    [JsonIgnore] public int Id { get; set; }

    [Required] public string Value { get; set; }

    [Required] [JsonIgnore] public string UserId { get; set; }

    [JsonIgnore] public IdentityUser User { get; set; }
}