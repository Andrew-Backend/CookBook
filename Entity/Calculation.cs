namespace Entity;

public class Calculation
{
    public int IdCalculation{get;set;}
    public int IdDish{get;set;}
    public int IdIngredients{get;set;}
    public int Count{get;set;}
    
    public Dish Dish{get;set;}
    public Ingredient Ingredient{get;set;}
}