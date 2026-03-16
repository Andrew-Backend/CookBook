namespace Entity;

public class Calculation
{
    public int Id{get;set;}
    public int DishId{get;set;}
    public int IngredientId{get;set;}
    public int CountDish{get;set;}
    
    public Dish Dish{get;set;}
    public Ingredient Ingredient{get;set;}
}