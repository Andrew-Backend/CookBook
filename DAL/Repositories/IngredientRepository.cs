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
                        IdIngredients = (int)reader["id_ingridients"],
                        NameIngredient = (string)reader["name_ingridients"],
                        IngredientPrice = (double)reader["ingridient_price"],
                        IdUnit = (int)reader["id_unit_meashuring"],
                        Unit = new UnitIngredients
                        {
                            IdUnit = (int)reader["id_unit_meashuring"],
                            NameUnit = (string)reader["name_unit_meashuring"]
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

            command.Parameters.AddWithValue("@name", ingredient.IdIngredients);
            command.Parameters.AddWithValue("@price", ingredient.IngredientPrice);
            command.Parameters.AddWithValue("@unitId", ingredient.IdUnit);

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

            command.Parameters.AddWithValue("@id", ingredient.IdIngredients);
            command.Parameters.AddWithValue("@name", ingredient.IdIngredients);
            command.Parameters.AddWithValue("@price", ingredient.IngredientPrice);
            command.Parameters.AddWithValue("@unitId", ingredient.IdUnit);

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
