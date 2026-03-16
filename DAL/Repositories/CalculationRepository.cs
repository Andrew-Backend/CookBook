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
                        Id = (int)reader["id_calculation"],
                        DishId = (int)reader["id_dish"],
                        IngredientId = (int)reader["id_ingridients"],
                        CountDish = (int)reader["count_dish"],
                        Ingredient = new Ingredient
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

            command.Parameters.AddWithValue("@dishId", calculation.DishId);
            command.Parameters.AddWithValue("@ingridientId", calculation.IngredientId);
            command.Parameters.AddWithValue("@count", calculation.CountDish);

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

            command.Parameters.AddWithValue("@id", calculation.Id);
            command.Parameters.AddWithValue("@ingridientId", calculation.IngredientId);
            command.Parameters.AddWithValue("@count", calculation.CountDish);

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