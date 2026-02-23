using DAL.Connection;
using Entity;
using Microsoft.Data.SqlClient;

namespace DAL;

public class DishRepository
{
        private readonly DbConnection _connection;
        
        public DishRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public List<Dish> GetDishes()
        {
            var result = new List<Dish>();

            using (var connection = _connection.CreateConnection())
            {
                connection.Open();

                var command = new SqlCommand("select \n" +
                                             "\td.IdDish,\n" +
                                             "\td.NameDish,\n" +
                                             "\td.DescriptionDish," +
                                             "\n\td.IdTypeDish," +
                                             "\n\tdt.DishType" +
                                             "\nfrom Dish d " +
                                             "\njoin Dish_type dt on d.IdDish = dt.IdDishType", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Dish
                        {
                            IdDish = (int)reader["IdDish"],
                            NameDish = (string)reader["NameDish"],
                            DescriptionDish =  (string)reader["DescriptionDish"],
                            IdTypeDish = (int)reader["IdTypeDish"],
                            DishType = new DishType
                            {
                                IdTypeDish =  (int)reader["IdTypeDish"],
                                NameDishType =   (string)reader["NameDishType"]
                            }
                        });
                    }
                }
            }
            
            return result;
        }

        public void Add(Dish dish)
        {
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();
                var command = new SqlCommand("insert into Dish (NameDish, DescriptionDish, IdTypeDish)" +
                                             "VALUES (@NameDish, @DescriptionDish, @IdTypeDish)", connection);

                command.Parameters.AddWithValue("@NameDish", dish.NameDish);
                command.Parameters.AddWithValue("@DescriptionDish", dish.DescriptionDish);
                command.Parameters.AddWithValue("@IdTypeDish", dish.IdTypeDish);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Dish dish)
        {
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();
                
                var command = new SqlCommand("update Dish set NameDish=@NameDish," +
                                             " DescriptionDish=@DescriptionDish," +
                                             " IdTypeDish=@IdTypeDish " +
                                             "where IdDish = @IdDish)", connection);
                command.Parameters.AddWithValue("@NameDish", dish.NameDish);
                command.Parameters.AddWithValue("@DescriptionDish", dish.DescriptionDish);
                command.Parameters.AddWithValue("@IdTypeDish", dish.IdTypeDish);
                
                command.ExecuteNonQuery();
            }
        }
        
        public void Delete(int idDish)
        {
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();
                
                var command = new SqlCommand("delete from Dish where IdDish = @IdDish", connection);
                
                command.Parameters.AddWithValue("@IdDish", idDish);
                
                command.ExecuteNonQuery();
            }
        }
}