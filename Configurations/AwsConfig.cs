namespace ContosoPizza.Configurations;

public class AwsConfig
{
    public string Profile { get; set; }
    public string Region { get; set; }
    public string PublicBucketName { get; set; }
    public string PrivateBucketName { get; set; }
}