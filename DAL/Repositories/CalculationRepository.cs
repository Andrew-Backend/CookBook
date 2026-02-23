using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class CalculationRepository
{
    
    private readonly DbConnection _connection;
    public CalculationRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public List<Calculation> GetByDish(int idDish)
    {
        var result = new List<Calculation>();

        using (var connection = _connection.CreateConnection())
        {
            connection.Open();

            var command = new SqlCommand("select \n" +
                                         "\tc.IdCalculation,\n" +
                                         "\td.IdDish,\n" +
                                         "\ti.IdIngridients,\n" +
                                         "\tc.CountDish\n" +
                                         "from Calculation c\n" +
                                         "join Dish d on c.IdDish = d.IdDish\n" +
                                         "join Ingridients i on c.IdIngridients = i.IdIngridients");
            
            command.Parameters.AddWithValue("@IdDish", idDish);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Calculation
                        {
                            IdCalculation = (int)reader["IdCalculation"],
                            IdDish = (int)reader["IdDish"],
                            IdIngredients = (int)reader["IdIngridients"],
                            Count = (int)reader["CountDish"],
                            Ingredient = new Ingredient
                            {
                                IdIngredients = (int)reader["IdIngridients"],
                                NameIngredient = (string)reader["NameIngridients"],
                                IngredientPrice = (double)reader["IngridientPrice"],
                                IdUnit = (int)reader["IdUnit"],
                                Unit = new UnitIngredients
                                {
                                    IdUnit = (int)reader["IdUnit"],
                                    NameUnit = (string)reader["NameUnit"]
                                }
                            }
                        }
                    );
                }
            }
        }
        return result;
    }

    public void AddCalculation(Calculation calculation)
    {
        using (var connection = _connection.CreateConnection())
        {
            connection.Open();
            
            var command = new SqlCommand("insert into Calculation(IdDish, IdIngridients, CountDish)" +
                                         "values(@IdDish, @IdIngridients, @CountDish)",  connection);
            command.Parameters.AddWithValue("@IdDish", calculation.IdDish);
            command.Parameters.AddWithValue("@IdIngredients", calculation.IdIngredients);
            command.Parameters.AddWithValue("@CountDish", calculation.IdDish);
            
            command.ExecuteNonQuery();
        }
    }
    
    public void UpdateCalculation(Calculation calculation)
    {
        using (var connection = _connection.CreateConnection())
        {
            connection.Open();

            var command = new SqlCommand("update Calculation set IdDish=@IdDish," +
                                         " IdIngredient=@IdIngredient," +
                                         " CountDish=@CountDish" +
                                         "where IdCalculation=@IdCalculation",  connection);
            
            command.Parameters.AddWithValue("@IdDish", calculation.IdDish);
            command.Parameters.AddWithValue("@IdIngredient", calculation.IdIngredients);
            command.Parameters.AddWithValue("@CountDish", calculation.IdDish);
            command.ExecuteNonQuery();
        }
    }
    
    public void DeleteCalculation(int id)
    {
        using (var connection = _connection.CreateConnection())
        {
            connection.Open();
            
            var command = new SqlCommand("delete from Calculation where IdCalculation=@IdCalculation",connection);
            
            command.Parameters.AddWithValue("@IdCalculation", id);
            command.ExecuteNonQuery();
        }
    }
    
}