namespace Entity;

public class Ingredient
{
    public int Id{get;set;}
    public string Name{get;set;}
    public double PricePerUnit{get;set;}
    
    public int UnitId{get;set;}
    public UnitIngredients Unit{get;set;}
}