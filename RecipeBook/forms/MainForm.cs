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

    // контролы
    private Button btnUnits;
    private Button btnIngredients;
    private Button btnDishTypes;
    private Button btnDishes;
    private Button btnReport;
    private Label lblTitle;

    public MainForm(UnitService unitService,
                    IngredientService ingredientService,
                    DishTypeService dishTypeService,
                    DishService dishService,
                    CalculationService calculationService,
                    OrderService orderService)
    {
        _unitService = unitService;
        _ingredientService = ingredientService;
        _dishTypeService = dishTypeService;
        _dishService = dishService;
        _calculationService = calculationService;
        _orderService = orderService;
        InitializeComponent();
        BuildUI();
    }

    private void BuildUI()
    {
        this.Text = "Книга рецептов";
        this.Size = new Size(400, 380);
        this.StartPosition = FormStartPosition.CenterScreen;

        lblTitle = new Label
        {
            Text = "Книга рецептов",
            Location = new Point(10, 20),
            Size = new Size(360, 35),
            Font = new Font(this.Font.FontFamily, 16, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };

        btnUnits = new Button
        {
            Text = "Единицы измерения",
            Location = new Point(50, 70),
            Size = new Size(280, 40)
        };

        btnIngredients = new Button
        {
            Text = "Ингредиенты",
            Location = new Point(50, 120),
            Size = new Size(280, 40)
        };

        btnDishTypes = new Button
        {
            Text = "Типы блюд",
            Location = new Point(50, 170),
            Size = new Size(280, 40)
        };

        btnDishes = new Button
        {
            Text = "Блюда",
            Location = new Point(50, 220),
            Size = new Size(280, 40)
        };

        btnReport = new Button
        {
            Text = "Сформировать отчёт",
            Location = new Point(50, 270),
            Size = new Size(280, 40),
            BackColor = Color.LightGreen
        };

        // события
        btnUnits.Click += btnUnits_Click;
        btnIngredients.Click += btnIngredients_Click;
        btnDishTypes.Click += btnDishTypes_Click;
        btnDishes.Click += btnDishes_Click;
        btnReport.Click += btnReport_Click;

        // добавляем на форму
        this.Controls.Add(lblTitle);
        this.Controls.Add(btnUnits);
        this.Controls.Add(btnIngredients);
        this.Controls.Add(btnDishTypes);
        this.Controls.Add(btnDishes);
        this.Controls.Add(btnReport);
    }

    private void btnUnits_Click(object sender, EventArgs e)
    {
        var form = new UnitForm(_unitService);
        form.ShowDialog();
    }

    private void btnIngredients_Click(object sender, EventArgs e)
    {
        var form = new IngredientForm(_ingredientService, _unitService);
        form.ShowDialog();
    }

    private void btnDishTypes_Click(object sender, EventArgs e)
    {
        var form = new DishTypeForm(_dishTypeService);
        form.ShowDialog();
    }

    private void btnDishes_Click(object sender, EventArgs e)
    {
        var form = new DishForm(_dishService, _dishTypeService,
                                _calculationService, _ingredientService);
        form.ShowDialog();
    }

    private void btnReport_Click(object sender, EventArgs e)
    {
        var form = new ReportForm(_dishService, _orderService);
        form.ShowDialog();
    }
}