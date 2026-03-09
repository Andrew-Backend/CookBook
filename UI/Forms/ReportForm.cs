using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class ReportForm : Form
{
    private readonly DishService _dishService;
    private readonly OrderService _orderService;

    // словарь выбранных блюд — ключ Dish, значение порции
    private Dictionary<Dish, int> _selectedDishes = new Dictionary<Dish, int>();

    public ReportForm(DishService dishService, OrderService orderService)
    {
        InitializeComponent();
        _dishService = dishService;
        _orderService = orderService;
    }

    private void ReportForm_Load(object sender, EventArgs e)
    {
        LoadDishes();
        numPortions.Minimum = 1;
        numPortions.Maximum = 100;
        numPortions.Value = 1;
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

    // добавить выбранное блюдо в словарь
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
            IdDish = (int)row.Cells["Id"].Value,
            NameDish = (string)row.Cells["Name"].Value
        };

        var portions = (int)numPortions.Value;

        if (_selectedDishes.ContainsKey(dish))
        {
            // если блюдо уже добавлено — обновляем порции
            _selectedDishes[dish] = portions;
        }
        else
        {
            _selectedDishes.Add(dish, portions);
        }

        RefreshSelectedDishes();
    }

    // удалить блюдо из словаря
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

    // обновить таблицу выбранных блюд
    private void RefreshSelectedDishes()
    {
        var list = _selectedDishes.Select(x => new
        {
            Id = x.Key.IdDish,
            Название = x.Key.NameDish,
            Порций = x.Value
        }).ToList();

        dgvSelected.DataSource = list;
        dgvSelected.Columns["Id"].Visible = false;
    }

    // сформировать отчёт
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

            // переименовываем колонки для красивого отображения
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