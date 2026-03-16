using System.Data;
using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class IngredientRepository
{
    private readonly DbConnection _factory;

    public IngredientRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<Ingredient> GetAll()
    {
        var result = new List<Ingredient>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_GetAllIngridients", connection);
            command.CommandType = CommandType.StoredProcedure;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Ingredient
                    {
                        Id = (int)reader["id_ingridients"],
                        Name = (string)reader["name_ingridients"],
                        PricePerUnit = Convert.ToDouble(reader["ingridient_price"]),
                        UnitId = (int)reader["id_unit_meashuring"],
                        Unit = new UnitIngredients
                        {
                            Id = (int)reader["id_unit_meashuring"],
                            Name = (string)reader["name_unit_meashuring"]
                        }
                    });
                }
            }
        }

        return result;
    }

    public void Add(Ingredient ingredient)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_AddIngridient", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", ingredient.Name);
            command.Parameters.AddWithValue("@price", ingredient.PricePerUnit);
            command.Parameters.AddWithValue("@unitId", ingredient.UnitId);

            command.ExecuteNonQuery();
        }
    }

    public void Update(Ingredient ingredient)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_UpdateIngridient", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", ingredient.Id);
            command.Parameters.AddWithValue("@name", ingredient.Name);
            command.Parameters.AddWithValue("@price", ingredient.PricePerUnit);
            command.Parameters.AddWithValue("@unitId", ingredient.UnitId);

            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("sp_DeleteIngridient", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }
}
