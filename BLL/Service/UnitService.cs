using DAL;
using Entity;

namespace BLL;

public class UnitService
{
    private readonly UnitRepository _unitRepository;

    public UnitService(UnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public List<UnitIngredients> GetAll()
    {
        return _unitRepository.GetAll();
    }

    public void Add(UnitIngredients unit)
    {
        if (string.IsNullOrWhiteSpace(unit.Name))
            throw new Exception("Название единицы измерения не может быть пустым");

        _unitRepository.Add(unit);
    }

    public void Delete(int id)
    {
        _unitRepository.Delete(id);
    }
}