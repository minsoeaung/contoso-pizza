using Amazon.S3;
using Amazon.S3.Model;
using ContosoPizza.Configurations;
using ContosoPizza.Data;
using ContosoPizza.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly ContosoContext _context;
    private readonly IAmazonS3 _s3;
    private readonly AwsConfig _awsConfig;

    public PizzaService(ContosoContext context, IAmazonS3 s3, IOptions<AwsConfig> awsConfig)
    {
        _context = context;
        _s3 = s3;
        _awsConfig = awsConfig.Value;
    }

    public IEnumerable<Pizza> GetAll()
    {
        return _context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .AsNoTracking()
            .ToList();
    }

    public Pizza? GetById(int id)
    {
        return _context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }

    public Pizza Create(Pizza newPizza)
    {
        _context.Pizzas.Add(newPizza);
        _context.SaveChanges();
        return newPizza;
    }

    public void UpdateSauce(int pizzaId, int sauceId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(pizzaId);
        var sauceToUpdate = _context.Sauces.Find(sauceId);

        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new InvalidOperationException("Pizza or sauce does not exist");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;

        _context.SaveChanges();
    }

    public void AddTopping(int pizzaId, int toppingId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(pizzaId);
        var toppingToAdd = _context.Toppings.Find(toppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exist");
        }

        if (pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        pizzaToUpdate.Toppings.Add(toppingToAdd);

        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizzaToDelete = _context.Pizzas.Find(id);
        if (pizzaToDelete is null) return;
        _context.Pizzas.Remove(pizzaToDelete);
        _context.SaveChanges();
    }

    public async Task<PutObjectResponse> UploadImageAsync(int id, IFormFile file)
    {
        var pubObjectRequest = new PutObjectRequest()
        {
            BucketName = _awsConfig.PublicBucketName,
            Key = $"pizza_images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(), // Better than ContentBody
            Metadata =
            {
                ["x-amz-meta-originalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName)
            }
        };

        return await _s3.PutObjectAsync(pubObjectRequest);
    }

    public async Task<GetObjectResponse?> GetImageAsync(int id)
    {
        try
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = _awsConfig.PublicBucketName,
                Key = $"pizza_images/{id}"
            };

            return await _s3.GetObjectAsync(getObjectRequest);
        }
        catch (AmazonS3Exception s3Exception) when (s3Exception.Message == "The specified key does not exist.")
        {
            return null;
        }
    }

    public async Task<DeleteObjectResponse> DeleteImageAsync(int id)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = _awsConfig.PublicBucketName,
            Key = $"pizza_images/{id}"
        };

        return await _s3.DeleteObjectAsync(deleteObjectRequest);
    }
}