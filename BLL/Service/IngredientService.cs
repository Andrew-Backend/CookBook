

using DAL;
using Entity;
using Microsoft.Data.SqlClient;

namespace BLL;

public class IngredientService
{
    private readonly IngredientRepository _ingredientRepository;

    public IngredientService(IngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }

    public List<Ingredient> GetAll()
    {
        return _ingredientRepository.GetAll();
    }

    public void Add(Ingredient ingredient)
    {
        if (string.IsNullOrWhiteSpace(ingredient.NameIngredient))
            throw new Exception("Название ингредиента не может быть пустым");

        if (ingredient.IngredientPrice <= 0)
            throw new Exception("Цена должна быть больше нуля");

        _ingredientRepository.Add(ingredient);
    }

    public void Update(Ingredient ingredient)
    {
        if (string.IsNullOrWhiteSpace(ingredient.NameIngredient))
            throw new Exception("Название ингредиента не может быть пустым");

        if (ingredient.IngredientPrice <= 0)
            throw new Exception("Цена должна быть больше нуля");

        _ingredientRepository.Update(ingredient);
    }

    public void Delete(int id)
    {
        try
        {
            _ingredientRepository.Delete(id);
        }
        catch (SqlException ex) when (ex.Number == 50002)
        {
            throw new Exception(ex.Message);
        }
    }
}