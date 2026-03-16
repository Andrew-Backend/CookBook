using System.Data;
using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class OrderedRepository
{
    private readonly DbConnection _factory;

    public OrderedRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<Ordered> GetByOrder(int orderId)
    {
        var result = new List<Ordered>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_GetOrderedByOrder", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@orderId", orderId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Ordered
                    {
                        Id = (int)reader["id_ordered"],
                        OrderId = (int)reader["id_orders"],
                        DishId = (int)reader["id_dish"],
                        Portions = (int)reader["portions"],
                        Dish = new Dish
                        {
                            Id = (int)reader["id_dish"],
                            Name = (string)reader["name_dish"]
                        }
                    });
                }
            }
        }

        return result;
    }

    public void Add(Ordered ordered)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_AddOrdered", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@orderId", ordered.OrderId);
            command.Parameters.AddWithValue("@dishId", ordered.DishId);
            command.Parameters.AddWithValue("@portions", ordered.Portions);

            command.ExecuteNonQuery();
        }
    }
}