using System.Data;
using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class OrderRepository
{
    private readonly DbConnection _factory;

    public OrderRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<Order> GetAll()
    {
        var result = new List<Order>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_GetAllOrders", connection);
            command.CommandType = CommandType.StoredProcedure;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Order
                    {
                        Id = (int)reader["id_orders"],
                        DateOrders = (DateTime)reader["date_orders"],
                        Result = reader["result"] as string
                    });
                }
            }
        }

        return result;
    }

    public int Add(Order order)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_AddOrder", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@date", order.DateOrders);
            command.Parameters.AddWithValue("@result", 
                (object)order.Result ?? DBNull.Value);

            return Convert.ToInt32(command.ExecuteScalar());
        }
    }

    public void Delete(int id)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_DeleteOrder", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}