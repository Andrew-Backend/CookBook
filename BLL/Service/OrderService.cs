using BLL.Model;
using DAL;
using Entity;

namespace BLL;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly OrderedRepository _orderedRepository;
    private readonly ReportService _reportService;

    public OrderService(OrderRepository orderRepository,
        OrderedRepository orderedRepository,
        ReportService reportService)
    {
        _orderRepository = orderRepository;
        _orderedRepository = orderedRepository;
        _reportService = reportService;
    }

    public List<Order> GetAll()
    {
        return _orderRepository.GetAll();
    }

    // создаём отчёт — сохраняем в БД и сразу генерируем
    public List<ReportItem> CreateAndGenerate(Dictionary<Dish, int> dishPortions)
    {
        if (dishPortions == null || dishPortions.Count == 0)
            throw new Exception("Выберите хотя бы одно блюдо");

        foreach (var entry in dishPortions)
        {
            if (entry.Value <= 0)
                throw new Exception($"Количество порций для '{entry.Key.NameDish}' должно быть больше нуля");
        }

        // 1 — сохраняем шапку заказа
        var order = new Order
        {
            DateOrders = DateTime.Now
        };
        int orderId = _orderRepository.Add(order);

        // 2 — сохраняем блюда с порциями
        foreach (var entry in dishPortions)
        {
            _orderedRepository.Add(new Ordered
            {
                OrderId = orderId,
                DishId = entry.Key.IdDish,
                Portions = entry.Value
            });
        }

        // 3 — генерируем и возвращаем отчёт
        return _reportService.Generate(orderId);
    }

    // загружаем уже сохранённый отчёт
    public List<ReportItem> LoadReport(int orderId)
    {
        return _reportService.Generate(orderId);
    }

    public void Delete(int id)
    {
        _orderRepository.Delete(id);
    }
}