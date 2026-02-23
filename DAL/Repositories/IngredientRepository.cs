using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class IngredientRepository
{
    
    private readonly DbConnection _connection;
    public IngredientRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public List<Ingredient> GetIngredient()
    {
        var result = new List<Ingredient>();

        using (var connection = _connection.CreateConnection())
        {
            connection.Open();

            var command = new SqlCommand("select \n" +
                                         "\ti.IdIngridients,\n" +
                                         "\ti.NameIngridients,\n" +
                                         "\ti.IngridientPrice,\n" +
                                         "\tu.NameUnit\n" +
                                         "from Ingridients i \n" +
                                         "join UnitIngridients u on i.IdIngridients = u.IdUnitIngridients");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Ingredient
                    {
                        IdIngredients = (int) reader["IdIngridients"],
                        NameIngredient =  (string)reader["NameIngridients"],
                        IngredientPrice = (double)reader["IngridientPrice"],
                        IdUnit = (int) reader["IdUnit"],
                        Unit = new UnitIngredients
                        {
                            IdUnit = (int) reader["IdUnit"],
                            NameUnit = (string) reader["NameUnit"]
                        }
                    });
                }
            }
        }
        return result;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        using (var connection = _connection.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("insert into Ingridients(NameIngridients, IdUnit, IngredientPrice)" +
                                         "values (@NameIngridients, @IdUnit, @IngredientPrice)",  connection);

            command.Parameters.AddWithValue("@NameIngridients", ingredient.NameIngredient);
            command.Parameters.AddWithValue("@IdUnit", ingredient.IdUnit);
            command.Parameters.AddWithValue("@IngredientPrice", ingredient.IngredientPrice);
            
            command.ExecuteNonQuery();
        }
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        using (var connection = _connection.CreateConnection())
        {
            connection.Open();
            var command = new SqlCommand("update Ingridients set NameIngredients=@NameIngredients," +
                                         "IdUnit=@IdUnit," +
                                         "IngredientPrice=@IngredientPrice " +
                                         "where IdIngredients=@IdIngredients", connection);
            command.Parameters.AddWithValue("@NameIngredients", ingredient.NameIngredient);
            command.Parameters.AddWithValue("@IdUnit", ingredient.IdUnit);
            command.Parameters.AddWithValue("@IngredientPrice", ingredient.IngredientPrice);

            command.ExecuteNonQuery();
        }
    }

    public void DeleteIngredient(Ingredient ingredient)
    {
        using (var connection = _connection.CreateConnection())
        {
            connection.Open();
            
            var command = new SqlCommand("delete from Ingredients where IdIngredients=@IdIngredients", connection);
            
            command.Parameters.AddWithValue("@IdIngredients", ingredient.IdIngredients);
            
            command.ExecuteNonQuery();
        }
    }
}