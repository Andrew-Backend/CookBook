

using DAL;
using Entity;

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
        return _ingredientRepository.GetIngredient();
    }

    public void Add(Ingredient ingredient)
    {
        if (string.IsNullOrWhiteSpace(ingredient.NameIngredient))
            throw new Exception("Название ингредиента не может быть пустым");

        _ingredientRepository.AddIngredient(ingredient);
    }
}