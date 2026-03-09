using System.Data;
using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class DishRepository
{
    private readonly DbConnection _factory;

    public DishRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<Dish> GetAll()
    {
        var result = new List<Dish>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_GetAllDishes", connection);
            command.CommandType = CommandType.StoredProcedure;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Dish
                    {
                        IdDish = (int)reader["id_dish"],
                        NameDish = (string)reader["name_dish"],
                        DescriptionDish = (string)reader["descriptions"],  // вместо Price
                        IdTypeDish = (int)reader["id_type_dish"],
                        DishType = new DishType
                        {
                            IdTypeDish = (int)reader["id_type_dish"],
                            NameDishType = (string)reader["dish_type"]
                        }
                    });
                }
            }
        }

        return result;
    }

    public void Add(Dish dish)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_AddDish", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", dish.NameDish);
            command.Parameters.AddWithValue("@descriptions", dish.DescriptionDish);
            command.Parameters.AddWithValue("@typeId", dish.IdTypeDish);

            command.ExecuteNonQuery();
        }
    }

    public void Update(Dish dish)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_UpdateDish", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", dish.IdDish);
            command.Parameters.AddWithValue("@name", dish.NameDish);
            command.Parameters.AddWithValue("@descriptions", dish.DescriptionDish);
            command.Parameters.AddWithValue("@typeId", dish.IdTypeDish);

            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_DeleteDish", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}