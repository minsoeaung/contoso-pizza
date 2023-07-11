namespace ContosoPizza.Models;

public class Coupon
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? Expiration { get; set; }
}