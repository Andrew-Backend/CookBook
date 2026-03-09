using System.Data;
using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class CalculationRepository
{
    private readonly DbConnection _factory;

    public CalculationRepository(DbConnection factory)
    {
        _factory = factory;
    }

    public List<Calculation> GetByDish(int dishId)
    {
        var result = new List<Calculation>();

        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("spGetCalculationDish", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@dishId", dishId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Calculation
                    {
                        IdCalculation = (int)reader["id_calculation"],
                        IdDish = (int)reader["id_dish"],
                        IdIngredients = (int)reader["id_ingridients"],
                        Count = (int)reader["count_dish"],
                        Ingredient = new Ingredient
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
                        }
                    });
                }
            }
        }

        return result;
    }

    public void Add(Calculation calculation)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("spAddCalculation", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@dishId", calculation.IdDish);
            command.Parameters.AddWithValue("@ingridientId", calculation.IdIngredients);
            command.Parameters.AddWithValue("@count", calculation.Count);

            command.ExecuteNonQuery();
        }
    }

    public void Update(Calculation calculation)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("spUpdateCalculation", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", calculation.IdCalculation);
            command.Parameters.AddWithValue("@ingridientId", calculation.IdIngredients);
            command.Parameters.AddWithValue("@count", calculation.Count);

            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = _factory.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("spDeleteCalculation", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}