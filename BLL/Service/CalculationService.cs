using DAL;
using Entity;

namespace BLL;

public class CalculationService
{
    private readonly CalculationRepository _calculationRepository;

    public CalculationService(CalculationRepository calculationRepository)
    {
        _calculationRepository = calculationRepository;
    }

    public List<Calculation> GetByDish(int dishId)
    {
        return _calculationRepository.GetByDish(dishId);
    }

    public void Add(Calculation calculation)
    {
        if (calculation.IdDish <= 0)
            throw new Exception("Блюдо не выбрано");

        if (calculation.IdIngredients <= 0)
            throw new Exception("Ингредиент не выбран");

        if (calculation.Count <= 0)
            throw new Exception("Количество должно быть больше нуля");

        _calculationRepository.Add(calculation);
    }

    public void Update(Calculation calculation)
    {
        if (calculation.Count <= 0)
            throw new Exception("Количество должно быть больше нуля");

        _calculationRepository.Update(calculation);
    }

    public void Delete(int id)
    {
        _calculationRepository.Delete(id);
    }
}