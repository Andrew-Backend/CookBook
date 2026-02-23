using System.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data.SqlClient;


namespace Recipe_Book
{
    internal static class Program
    {
        
        //[STAThread]
        static void Main()
        {
            
            var connectionString = ConfigurationManager.ConnectionStrings["RecipeBook"].ConnectionString;
            var factory = new DbConnectionFactory(connectionString);
            SqlConnection conection = new SqlConnection(connectionString);

            try
            {
                conection.Open();
                Console.WriteLine(conection.State);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally {
                conection.Close();
                Console.WriteLine(conection.State);
            }
            //string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //Console.WriteLine(connectionString);

            //Console.Read();
            // To customize application configuration such as set high DPI settings or default font,
            //// see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
        }
    }
}