namespace Entity;

public class Ingredient
{
    public int IdIngredients{get;set;}
    public string NameIngredient{get;set;}
    public int IdUnit{get;set;}
    public double IngredientPrice{get;set;}
    
    public UnitIngredients Unit{get;set;}
}