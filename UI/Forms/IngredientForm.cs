using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class IngredientForm : Form
{
    private readonly IngredientService _ingredientService;
    private readonly UnitService _unitService;
    private int? _selectedId = null;

    public IngredientForm(IngredientService ingredientService, UnitService unitService)
    {
        InitializeComponent();
        _ingredientService = ingredientService;
        _unitService = unitService;
    }

    private void IngredientForm_Load(object sender, EventArgs e)
    {
        LoadUnitsToComboBox();
        LoadIngredients();
    }

    private void LoadUnitsToComboBox()
    {
        var units = _unitService.GetAll();
        cmbUnit.DataSource = units;
        cmbUnit.DisplayMember = "Name";  // что показывать
        cmbUnit.ValueMember = "Id";      // что использовать как значение
    }

    private void LoadIngredients()
    {
        var ingredients = _ingredientService.GetAll();
        dgvIngredients.DataSource = ingredients;

        dgvIngredients.Columns["Id"].Visible = false;
        dgvIngredients.Columns["UnitId"].Visible = false;
        dgvIngredients.Columns["Unit"].Visible = false;
    }

   private void dgvIngredients_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvIngredients.SelectedRows.Count == 0) return;

        var row = dgvIngredients.SelectedRows[0];
        _selectedId = (int)row.Cells["Id"].Value;
        txtName.Text = (string)row.Cells["Name"].Value;
        txtPrice.Text = row.Cells["PricePerUnit"].Value.ToString();
        cmbUnit.SelectedValue = (int)row.Cells["UnitId"].Value;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var ingredient = new Ingredient
            {
                NameIngredient = txtName.Text.Trim(),
                IngredientPrice = decimal.Parse(txtPrice.Text),
                IdUnit = (int)cmbUnit.SelectedValue
            };

            _ingredientService.Add(ingredient);
            ClearForm();
            LoadIngredients();
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
            var ingredient = new Ingredient
            {
                IdIngredients = _selectedId.Value,
                NameIngredient = txtName.Text.Trim(),
                IngredientPrice = decimal.Parse(txtPrice.Text),
                IdUnit = (int)cmbUnit.SelectedValue
            };

            _ingredientService.Update(ingredient);
            ClearForm();
            LoadIngredients();
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

        var confirm = MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
        {
            try
            {
                _ingredientService.Delete(_selectedId.Value);
                ClearForm();
                LoadIngredients();
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
        txtName.Clear();
        txtPrice.Clear();
        _selectedId = null;
        dgvIngredients.ClearSelection();
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;
    }
}