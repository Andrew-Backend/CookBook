using DAL;
using Entity;

namespace BLL;

public class DishService
{
    private readonly DishRepository _dishRepository;

    public DishService(DishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public List<Dish> GetAll()
    {
        return _dishRepository.GetAll();
    }

    public void Add(Dish dish)
    {
        if (string.IsNullOrWhiteSpace(dish.NameDish))
            throw new Exception("Название блюда не может быть пустым");

        if (dish.IdTypeDish <= 0)
            throw new Exception("Выберите тип блюда");

        _dishRepository.Add(dish);
    }

    public void Update(Dish dish)
    {
        if (string.IsNullOrWhiteSpace(dish.NameDish))
            throw new Exception("Название блюда не может быть пустым");

        if (dish.IdTypeDish <= 0)
            throw new Exception("Выберите тип блюда");

        _dishRepository.Update(dish);
    }

    public void Delete(int id)
    {
        _dishRepository.Delete(id);
    }
}