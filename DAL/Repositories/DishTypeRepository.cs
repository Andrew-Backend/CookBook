using System.Data;
using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class DishTypeRepository
{
    private readonly DbConnection _factory;

    public DishTypeRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<DishType> GetAll()
    {
        var result = new List<DishType>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_GetAllDishTypes", connection);
            command.CommandType = CommandType.StoredProcedure;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new DishType
                    {
                        IdTypeDish = (int)reader["id_dish_type"],
                        NameDishType = (string)reader["dish_type"]
                    });
                }
            }
        }

        return result;
    }

    public void Add(DishType dishType)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_AddDishType", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", dishType.NameDishType);
            command.ExecuteNonQuery();
        }
    }

    public void Update(DishType dishType)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_UpdateDishType", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", dishType.IdTypeDish);
            command.Parameters.AddWithValue("@name", dishType.NameDishType);
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_DeleteDishType", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}