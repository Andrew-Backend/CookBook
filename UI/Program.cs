using System.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data.SqlClient;
using BLL;
using DAL;
using DAL.Connection;
using Recipe_Book.Forms;


namespace Recipe_Book
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // строка подключения из App.config
            var connectionString = ConfigurationManager
                .ConnectionStrings["RecipeBook"].ConnectionString;

            // DbConnectionFactory
            var factory = new DbConnection(connectionString);

            // репозитории
            var unitRepository           = new UnitRepository(factory);
            var ingredientRepository     = new IngredientRepository(factory);
            var dishTypeRepository       = new DishTypeRepository(factory);
            var dishRepository           = new DishRepository(factory);
            var calculationRepository    = new CalculationRepository(factory);
            var orderRepository          = new OrderRepository(factory);
            var orderedRepository        = new OrderedRepository(factory);
            var reportRepository         = new ReportRepository(factory);

            // сервисы
            var unitService          = new UnitService(unitRepository);
            var ingredientService    = new IngredientService(ingredientRepository);
            var dishTypeService      = new DishTypeService(dishTypeRepository);
            var dishService          = new DishService(dishRepository);
            var calculationService   = new CalculationService(calculationRepository);
            var reportService        = new ReportService(reportRepository);
            var orderService         = new OrderService(orderRepository, 
                orderedRepository, 
                reportService);

            // запуск главной формы
            Application.Run(new MainForm(
                unitService,
                ingredientService,
                dishTypeService,
                dishService,
                calculationService,
                orderService
            ));
        }
    }
}