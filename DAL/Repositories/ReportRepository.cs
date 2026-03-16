
using System.Data;
using BLL.Model;
using DAL.Connection;
using Microsoft.Data.SqlClient;

namespace DAL;

public class ReportRepository
{
    private readonly DbConnection _factory;

    public ReportRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<ReportItem> Generate(int orderId)
    {
        var result = new List<ReportItem>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_GenerateReport", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@orderId", orderId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new ReportItem
                    {
                        IngredientName = (string)reader["name_ingridients"],
                        UnitName = (string)reader["name_unit_meashuring"],
                        TotalQuantity = Convert.ToDouble(reader["total_quantity"]),  
                        TotalCost = Convert.ToDouble(reader["total_cost"])           
                    });
                }
            }
        }

        return result;
    }
}
