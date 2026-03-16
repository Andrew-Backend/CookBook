namespace Entity;

public class Dish
{
     public int Id{get;set;}
     public string Name{get;set;}
     public string Description{get;set;}
     public int DishTypeId{get;set;}
     
     public DishType DishType{get;set;}
}