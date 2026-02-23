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
        if (calculation.Count <= 0)
            throw new Exception("Количество должно быть больше нуля");

        _calculationRepository.AddCalculation(calculation);
    }

    public void Update(Calculation dishIngredient)
    {
        if (dishIngredient.Count <= 0)
            throw new Exception("Количество должно быть больше нуля");

        _calculationRepository.UpdateCalculation(dishIngredient);
    }

    public void Delete(int id)
    {
        _calculationRepository.DeleteCalculation(id);
    }
}