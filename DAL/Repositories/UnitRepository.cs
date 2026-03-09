using System.Data;
using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class UnitRepository
{
    private readonly DbConnection _factory;

    public UnitRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<UnitIngredients> GetAll()
    {
        var result = new List<UnitIngredients>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_GetAllUnits", connection);
            command.CommandType = CommandType.StoredProcedure;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new UnitIngredients
                    {
                        IdUnit = (int)reader["id_unit_ingridients"],
                        NameUnit = (string)reader["name_unit_meashuring"]
                    });
                }
            }
        }

        return result;
    }

    public void Add(UnitIngredients unit)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_AddUnit", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", unit.NameUnit);
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_DeleteUnit", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}