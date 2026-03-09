namespace Entity;

public class Ordered
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int DishId { get; set; }
    public int Portions { get; set; }

    public Dish Dish { get; set; }
}