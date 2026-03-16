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
                        Id = (int)reader["id_dish"],
                        Name = (string)reader["name_dish"],
                        Description = (string)reader["descriptions"],  // вместо Price
                        DishTypeId = (int)reader["id_type_dish"],
                        DishType = new DishType
                        {
                            Id = (int)reader["id_type_dish"],
                            Name = (string)reader["dish_type"]
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

            command.Parameters.AddWithValue("@name", dish.Name);
            command.Parameters.AddWithValue("@descriptions", dish.Description);
            command.Parameters.AddWithValue("@typeId", dish.DishTypeId);

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

            command.Parameters.AddWithValue("@id", dish.Id);
            command.Parameters.AddWithValue("@name", dish.Name);
            command.Parameters.AddWithValue("@descriptions", dish.Description);
            command.Parameters.AddWithValue("@typeId", dish.DishTypeId);

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