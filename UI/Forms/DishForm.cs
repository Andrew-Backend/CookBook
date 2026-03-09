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

    public DishForm(DishService dishService, DishTypeService dishTypeService,
                    CalculationService calculationService, IngredientService ingredientService)
    {
        InitializeComponent();
        _dishService = dishService;
        _dishTypeService = dishTypeService;
        _calculationService = calculationService;
        _ingredientService = ingredientService;
    }

    private void DishForm_Load(object sender, EventArgs e)
    {
        LoadDishTypesToComboBox();
        LoadDishes();
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
                NameDish = txtName.Text.Trim(),
                DescriptionDish = txtDescription.Text.Trim(),
                IdTypeDish = (int)cmbDishType.SelectedValue
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
                IdDish = _selectedId.Value,
                NameDish = txtName.Text.Trim(),
                DescriptionDish = txtDescription.Text.Trim(),
                IdTypeDish = (int)cmbDishType.SelectedValue
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

    // открываем форму состава для выбранного блюда
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
            IdDish = _selectedId.Value,
            NameDish = (string)dgvDishes.SelectedRows[0].Cells["Name"].Value
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
