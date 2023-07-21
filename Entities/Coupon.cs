namespace ContosoPizza.Entities;

public class Coupon
{
    public long Id { get; set; }

    public required string Description { get; set; }

    public DateTime? Expiration { get; set; }
}