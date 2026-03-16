using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class ReportForm : Form
{
    private readonly DishService _dishService;
    private readonly OrderService _orderService;
    private Dictionary<Dish, int> _selectedDishes = new Dictionary<Dish, int>();

    // контролы
    private DataGridView dgvDishes;
    private DataGridView dgvSelected;
    private DataGridView dgvReport;
    private NumericUpDown numPortions;
    private Button btnAddToDish;
    private Button btnRemoveFromDish;
    private Button btnGenerate;
    private Label lblDishes;
    private Label lblSelected;
    private Label lblReport;
    private Label lblPortions;

    public ReportForm(DishService dishService, OrderService orderService)
    {
        _dishService = dishService;
        _orderService = orderService;
        InitializeComponent();
        BuildUI();
        LoadDishes();
    }

    private void BuildUI()
    {
        this.Text = "Формирование отчёта";
        this.Size = new Size(900, 600);

        // левая часть — все блюда
        lblDishes = new Label
        {
            Text = "Все блюда:",
            Location = new Point(10, 10),
            Size = new Size(200, 20)
        };

        dgvDishes = new DataGridView
        {
            Location = new Point(10, 30),
            Size = new Size(380, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false
        };

        // правая часть — выбранные блюда
        lblSelected = new Label
        {
            Text = "Выбранные блюда:",
            Location = new Point(400, 10),
            Size = new Size(200, 20)
        };

        dgvSelected = new DataGridView
        {
            Location = new Point(400, 30),
            Size = new Size(470, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false
        };

        // панель управления
        lblPortions = new Label
        {
            Text = "Количество порций:",
            Location = new Point(10, 295),
            Size = new Size(140, 25)
        };

        numPortions = new NumericUpDown
        {
            Location = new Point(160, 293),
            Size = new Size(70, 25),
            Minimum = 1,
            Maximum = 100,
            Value = 1
        };

        btnAddToDish = new Button
        {
            Text = "Добавить в отчёт →",
            Location = new Point(240, 292),
            Size = new Size(140, 30)
        };

        btnRemoveFromDish = new Button
        {
            Text = "← Удалить из отчёта",
            Location = new Point(390, 292),
            Size = new Size(150, 30)
        };

        btnGenerate = new Button
        {
            Text = "Сформировать отчёт",
            Location = new Point(550, 292),
            Size = new Size(160, 30),
            BackColor = Color.LightGreen
        };

        // нижняя часть — результат отчёта
        lblReport = new Label
        {
            Text = "Результат отчёта:",
            Location = new Point(10, 335),
            Size = new Size(200, 20)
        };

        dgvReport = new DataGridView
        {
            Location = new Point(10, 355),
            Size = new Size(860, 180),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AllowUserToAddRows = false
        };

        // события
        btnAddToDish.Click += btnAddToDish_Click;
        btnRemoveFromDish.Click += btnRemoveFromDish_Click;
        btnGenerate.Click += btnGenerate_Click;

        // добавляем на форму
        this.Controls.Add(lblDishes);
        this.Controls.Add(dgvDishes);
        this.Controls.Add(lblSelected);
        this.Controls.Add(dgvSelected);
        this.Controls.Add(lblPortions);
        this.Controls.Add(numPortions);
        this.Controls.Add(btnAddToDish);
        this.Controls.Add(btnRemoveFromDish);
        this.Controls.Add(btnGenerate);
        this.Controls.Add(lblReport);
        this.Controls.Add(dgvReport);
    }

    private void LoadDishes()
    {
        var dishes = _dishService.GetAll();
        dgvDishes.DataSource = dishes;

        dgvDishes.Columns["Id"].Visible = false;
        dgvDishes.Columns["DishTypeId"].Visible = false;
        dgvDishes.Columns["DishType"].Visible = false;
        dgvDishes.Columns["Description"].Visible = false;
    }

    private void btnAddToDish_Click(object sender, EventArgs e)
    {
        if (dgvDishes.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите блюдо", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var row = dgvDishes.SelectedRows[0];
        var dish = new Dish
        {
            Id = (int)row.Cells["Id"].Value,
            Name = (string)row.Cells["Name"].Value
        };

        var portions = (int)numPortions.Value;

        if (_selectedDishes.ContainsKey(dish))
            _selectedDishes[dish] = portions;
        else
            _selectedDishes.Add(dish, portions);

        RefreshSelectedDishes();
    }

    private void btnRemoveFromDish_Click(object sender, EventArgs e)
    {
        if (dgvSelected.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите блюдо для удаления", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var dishId = (int)dgvSelected.SelectedRows[0].Cells["Id"].Value;
        var dish = _selectedDishes.Keys.FirstOrDefault(d => d.Id == dishId);

        if (dish != null)
        {
            _selectedDishes.Remove(dish);
            RefreshSelectedDishes();
        }
    }

    private void RefreshSelectedDishes()
    {
        var list = _selectedDishes.Select(x => new
        {
            Id = x.Key.Id,
            Название = x.Key.Name,
            Порций = x.Value
        }).ToList();

        dgvSelected.DataSource = list;
        dgvSelected.Columns["Id"].Visible = false;
    }

    private void btnGenerate_Click(object sender, EventArgs e)
    {
        if (_selectedDishes.Count == 0)
        {
            MessageBox.Show("Добавьте хотя бы одно блюдо", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var report = _orderService.CreateAndGenerate(_selectedDishes);

            dgvReport.DataSource = report;

            dgvReport.Columns["IngredientName"].HeaderText = "Ингредиент";
            dgvReport.Columns["UnitName"].HeaderText = "Ед. изм.";
            dgvReport.Columns["TotalQuantity"].HeaderText = "Количество";
            dgvReport.Columns["TotalCost"].HeaderText = "Стоимость";

            MessageBox.Show("Отчёт сформирован и сохранён!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}