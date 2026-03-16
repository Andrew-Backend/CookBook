using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class DishForm : Form
{
    private readonly DishService _dishService;
    private readonly DishTypeService _dishTypeService;
    private readonly CalculationService _calculationService;
    private readonly IngredientService _ingredientService;
    private int? _selectedId = null;

    // контролы
    private DataGridView dgvDishes;
    private TextBox txtName;
    private TextBox txtDescription;
    private ComboBox cmbDishType;
    private Button btnAdd;
    private Button btnSave;
    private Button btnDelete;
    private Button btnCalculation;

    public DishForm(DishService dishService, DishTypeService dishTypeService,
                    CalculationService calculationService, IngredientService ingredientService)
    {
        _dishService = dishService;
        _dishTypeService = dishTypeService;
        _calculationService = calculationService;
        _ingredientService = ingredientService;
        InitializeComponent();
        BuildUI();
        LoadDishTypesToComboBox();
        LoadDishes();
    }

    private void BuildUI()
    {
        this.Text = "Блюда";
        this.Size = new Size(650, 500);

        dgvDishes = new DataGridView
        {
            Location = new Point(10, 10),
            Size = new Size(610, 270),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false
        };

        var lblName = new Label
        {
            Text = "Название:",
            Location = new Point(10, 295),
            Size = new Size(80, 25)
        };

        txtName = new TextBox
        {
            Location = new Point(100, 292),
            Size = new Size(180, 25),
            PlaceholderText = "Название блюда"
        };

        var lblDescription = new Label
        {
            Text = "Описание:",
            Location = new Point(10, 330),
            Size = new Size(80, 25)
        };

        txtDescription = new TextBox
        {
            Location = new Point(100, 327),
            Size = new Size(180, 25),
            PlaceholderText = "Описание блюда"
        };

        var lblDishType = new Label
        {
            Text = "Тип блюда:",
            Location = new Point(10, 365),
            Size = new Size(80, 25)
        };

        cmbDishType = new ComboBox
        {
            Location = new Point(100, 362),
            Size = new Size(180, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        btnAdd = new Button
        {
            Text = "Добавить",
            Location = new Point(300, 292),
            Size = new Size(90, 30)
        };

        btnSave = new Button
        {
            Text = "Сохранить",
            Location = new Point(400, 292),
            Size = new Size(90, 30)
        };

        btnDelete = new Button
        {
            Text = "Удалить",
            Location = new Point(500, 292),
            Size = new Size(90, 30)
        };

        btnCalculation = new Button
        {
            Text = "Состав",
            Location = new Point(300, 332),
            Size = new Size(90, 30)
        };

        // события
        dgvDishes.SelectionChanged += dgvDishes_SelectionChanged;
        btnAdd.Click += btnAdd_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnCalculation.Click += btnCalculation_Click;

        // добавляем на форму
        this.Controls.Add(dgvDishes);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(lblDescription);
        this.Controls.Add(txtDescription);
        this.Controls.Add(lblDishType);
        this.Controls.Add(cmbDishType);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnDelete);
        this.Controls.Add(btnCalculation);
    }

    private void LoadDishTypesToComboBox()
    {
        var types = _dishTypeService.GetAll();
        cmbDishType.DataSource = types;
        cmbDishType.DisplayMember = "Name";
        cmbDishType.ValueMember = "Id";
    }

    private void LoadDishes()
    {
        var dishes = _dishService.GetAll();
        dgvDishes.DataSource = dishes;

        dgvDishes.Columns["Id"].Visible = false;
        dgvDishes.Columns["DishTypeId"].Visible = false;
        dgvDishes.Columns["DishType"].Visible = false;
    }

    private void dgvDishes_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvDishes.SelectedRows.Count == 0) return;

        var row = dgvDishes.SelectedRows[0];
        _selectedId = (int)row.Cells["Id"].Value;
        txtName.Text = (string)row.Cells["Name"].Value;
        txtDescription.Text = (string)row.Cells["Description"].Value;
        cmbDishType.SelectedValue = (int)row.Cells["DishTypeId"].Value;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var dish = new Dish
            {
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                DishTypeId = (int)cmbDishType.SelectedValue
            };

            _dishService.Add(dish);
            ClearForm();
            LoadDishes();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (_selectedId == null)
        {
            MessageBox.Show("Выберите блюдо для редактирования", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var dish = new Dish
            {
                Id = _selectedId.Value,
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                DishTypeId = (int)cmbDishType.SelectedValue
            };

            _dishService.Update(dish);
            ClearForm();
            LoadDishes();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (_selectedId == null)
        {
            MessageBox.Show("Выберите блюдо для удаления", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show("Удалить блюдо?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
        {
            try
            {
                _dishService.Delete(_selectedId.Value);
                ClearForm();
                LoadDishes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void btnCalculation_Click(object sender, EventArgs e)
    {
        if (_selectedId == null)
        {
            MessageBox.Show("Выберите блюдо", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedDish = new Dish
        {
            Id = _selectedId.Value,
            Name = (string)dgvDishes.SelectedRows[0].Cells["Name"].Value
        };

        var form = new CalculationForm(selectedDish, _calculationService, _ingredientService);
        form.ShowDialog();
    }

    private void ClearForm()
    {
        txtName.Clear();
        txtDescription.Clear();
        _selectedId = null;
        dgvDishes.ClearSelection();
        if (cmbDishType.Items.Count > 0)
            cmbDishType.SelectedIndex = 0;
    }
}