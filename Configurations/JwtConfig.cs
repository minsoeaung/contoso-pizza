namespace ContosoPizza.Configurations;

public class JwtConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}