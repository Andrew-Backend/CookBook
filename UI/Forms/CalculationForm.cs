using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class CalculationForm : Form
{
    private readonly Dish _dish;
    private readonly CalculationService _calculationService;
    private readonly IngredientService _ingredientService;
    private int? _selectedId = null;

    public CalculationForm(Dish dish, CalculationService calculationService,
                           IngredientService ingredientService)
    {
        InitializeComponent();
        _dish = dish;
        _calculationService = calculationService;
        _ingredientService = ingredientService;
    }

    private void CalculationForm_Load(object sender, EventArgs e)
    {
        lblDishName.Text = $"Состав блюда: {_dish.NameDish}";
        LoadIngredientsToComboBox();
        LoadCalculation();
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
        dgvCalculation.DataSource = items;

        dgvCalculation.Columns["Id"].Visible = false;
        dgvCalculation.Columns["DishId"].Visible = false;
        dgvCalculation.Columns["IngredientId"].Visible = false;
        dgvCalculation.Columns["Ingredient"].Visible = false;
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
                IdDish = _dish.IdDish,
                IdIngredients = (int)cmbIngredient.SelectedValue,
                Count = decimal.Parse(txtCount.Text)
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
                IdCalculation = _selectedId.Value,
                IdIngredients = (int)cmbIngredient.SelectedValue,
                Count = decimal.Parse(txtCount.Text)
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