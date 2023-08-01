using Amazon.S3.Model;
using ContosoPizza.Entities;

namespace ContosoPizza.Services;

public interface IPizzaService
{
    IEnumerable<Pizza> GetAll();

    Pizza? GetById(int id);

    Pizza Create(Pizza newPizza);

    void UpdateSauce(int pizzaId, int sauceId);

    void AddTopping(int pizzaId, int toppingId);

    void DeleteById(int id);

    Task<PutObjectResponse> UploadImageAsync(int id, IFormFile file);
}