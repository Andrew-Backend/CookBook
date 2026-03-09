namespace Entity;

public class Dish
{
     public int IdDish{get;set;}
     public string NameDish{get;set;}
     public string DescriptionDish{get;set;}
     public int IdTypeDish{get;set;}
     
     public DishType DishType{get;set;}
}