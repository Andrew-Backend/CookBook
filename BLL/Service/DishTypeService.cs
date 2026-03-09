using DAL;
using Entity;
using Microsoft.Data.SqlClient;

namespace BLL;

public class DishTypeService
{
    private readonly DishTypeRepository _dishTypeRepository;

    public DishTypeService(DishTypeRepository dishTypeRepository)
    {
        _dishTypeRepository = dishTypeRepository;
    }

    public List<DishType> GetAll()
    {
        return _dishTypeRepository.GetAll();
    }

    public void Add(DishType dishType)
    {
        if (string.IsNullOrWhiteSpace(dishType.NameDishType))
            throw new Exception("Название типа блюда не может быть пустым");

        _dishTypeRepository.Add(dishType);
    }

    public void Update(DishType dishType)
    {
        if (string.IsNullOrWhiteSpace(dishType.NameDishType))
            throw new Exception("Название типа блюда не может быть пустым");

        _dishTypeRepository.Update(dishType);
    }

    public void Delete(int id)
    {
        try
        {
            _dishTypeRepository.Delete(id);
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new Exception(ex.Message);
        }
    }
}