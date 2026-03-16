using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class CalculationForm : Form
{
    private readonly Dish _dish;
    private readonly CalculationService _calculationService;
    private readonly IngredientService _ingredientService;
    private int? _selectedId = null;

    // контролы
    private DataGridView dgvCalculation;
    private ComboBox cmbIngredient;
    private TextBox txtCount;
    private Button btnAdd;
    private Button btnSave;
    private Button btnDelete;
    private Label lblDishName;

    public CalculationForm(Dish dish, CalculationService calculationService,
                           IngredientService ingredientService)
    {
        _dish = dish;
        _calculationService = calculationService;
        _ingredientService = ingredientService;
        InitializeComponent();
        BuildUI();
        LoadIngredientsToComboBox();
        LoadCalculation();
    }

    private void BuildUI()
    {
        this.Text = "Состав блюда";
        this.Size = new Size(600, 450);

        lblDishName = new Label
        {
            Text = $"Состав блюда: {_dish.Name}",
            Location = new Point(10, 10),
            Size = new Size(560, 25),
            Font = new Font(this.Font, FontStyle.Bold)
        };

        dgvCalculation = new DataGridView
        {
            Location = new Point(10, 40),
            Size = new Size(560, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false
        };

        var lblIngredient = new Label
        {
            Text = "Ингредиент:",
            Location = new Point(10, 305),
            Size = new Size(90, 25)
        };

        cmbIngredient = new ComboBox
        {
            Location = new Point(110, 302),
            Size = new Size(180, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        var lblCount = new Label
        {
            Text = "Количество:",
            Location = new Point(10, 340),
            Size = new Size(90, 25)
        };

        txtCount = new TextBox
        {
            Location = new Point(110, 337),
            Size = new Size(180, 25),
            PlaceholderText = "Количество на порцию"
        };

        btnAdd = new Button
        {
            Text = "Добавить",
            Location = new Point(305, 302),
            Size = new Size(90, 30)
        };

        btnSave = new Button
        {
            Text = "Сохранить",
            Location = new Point(405, 302),
            Size = new Size(90, 30)
        };

        btnDelete = new Button
        {
            Text = "Удалить",
            Location = new Point(305, 342),
            Size = new Size(90, 30)
        };

        // события
        dgvCalculation.SelectionChanged += dgvCalculation_SelectionChanged;
        btnAdd.Click += btnAdd_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;

        // добавляем на форму
        this.Controls.Add(lblDishName);
        this.Controls.Add(dgvCalculation);
        this.Controls.Add(lblIngredient);
        this.Controls.Add(cmbIngredient);
        this.Controls.Add(lblCount);
        this.Controls.Add(txtCount);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnDelete);
    }

    private void LoadIngredientsToComboBox()
    {
        var ingredients = _ingredientService.GetAll();
        cmbIngredient.DataSource = ingredients;
        cmbIngredient.DisplayMember = "Name";
        cmbIngredient.ValueMember = "Id";
    }

    private void LoadCalculation()
    {
        var items = _calculationService.GetByDish(_dish.Id);

        // создаём плоский список для отображения
        var display = items.Select(x => new
        {
            x.Id,
            x.DishId,
            x.IngredientId,
            Ингредиент = x.Ingredient?.Name,
            x.CountDish
        }).ToList();

        dgvCalculation.DataSource = display;

        dgvCalculation.Columns["Id"].Visible = false;
        dgvCalculation.Columns["DishId"].Visible = false;
        dgvCalculation.Columns["IngredientId"].Visible = false;
        dgvCalculation.Columns["CountDish"].HeaderText = "Количество на порцию";
    }

    private void dgvCalculation_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvCalculation.SelectedRows.Count == 0) return;

        var row = dgvCalculation.SelectedRows[0];
        _selectedId = (int)row.Cells["Id"].Value;
        cmbIngredient.SelectedValue = (int)row.Cells["IngredientId"].Value;
        txtCount.Text = row.Cells["CountDish"].Value.ToString();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var calculation = new Calculation
            {
                DishId = _dish.Id,
                IngredientId = (int)cmbIngredient.SelectedValue,
                CountDish = int.Parse(txtCount.Text)
            };

            _calculationService.Add(calculation);
            ClearForm();
            LoadCalculation();
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
            MessageBox.Show("Выберите запись для редактирования", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var calculation = new Calculation
            {
                Id = _selectedId.Value,      // ← было DishId = _selectedId.Value
                IngredientId = (int)cmbIngredient.SelectedValue,
                CountDish = int.Parse(txtCount.Text)
            };

            _calculationService.Update(calculation);
            ClearForm();
            LoadCalculation();
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
            MessageBox.Show("Выберите запись для удаления", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show("Удалить запись?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
        {
            try
            {
                _calculationService.Delete(_selectedId.Value);
                ClearForm();
                LoadCalculation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ClearForm()
    {
        txtCount.Clear();
        _selectedId = null;
        dgvCalculation.ClearSelection();
        if (cmbIngredient.Items.Count > 0)
            cmbIngredient.SelectedIndex = 0;
    }
}