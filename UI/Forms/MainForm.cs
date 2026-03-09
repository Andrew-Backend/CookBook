using BLL;

namespace Recipe_Book.Forms;

public partial class MainForm : Form
{
    private readonly UnitService _unitService;
    private readonly IngredientService _ingredientService;
    private readonly DishTypeService _dishTypeService;
    private readonly DishService _dishService;
    private readonly CalculationService _calculationService;
    private readonly OrderService _orderService;

    public MainForm(UnitService unitService,
        IngredientService ingredientService,
        DishTypeService dishTypeService,
        DishService dishService,
        CalculationService calculationService,
        OrderService orderService)
    {
        InitializeComponent();
        _unitService = unitService;
        _ingredientService = ingredientService;
        _dishTypeService = dishTypeService;
        _dishService = dishService;
        _calculationService = calculationService;
        _orderService = orderService;
    }

    private void btnDishes_Click(object sender, EventArgs e)
    {
        var form = new DishForm(_dishService, _dishTypeService, _calculationService, _ingredientService);
        form.ShowDialog();
    }

    private void btnIngredients_Click(object sender, EventArgs e)
    {
        var form = new IngredientForm(_ingredientService, _unitService);
        form.ShowDialog();
    }

    private void btnUnits_Click(object sender, EventArgs e)
    {
        var form = new UnitForm(_unitService);
        form.ShowDialog();
    }

    private void btnDishTypes_Click(object sender, EventArgs e)
    {
        var form = new DishTypeForm(_dishTypeService);
        form.ShowDialog();
    }

    private void btnReport_Click(object sender, EventArgs e)
    {
        var form = new ReportForm(_dishService, _orderService);
        form.ShowDialog();
    }
}
