using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class IngredientForm : Form
{
    private readonly IngredientService _ingredientService;
    private readonly UnitService _unitService;
    private int? _selectedId = null;

    // контролы
    private DataGridView dgvIngredients;
    private TextBox txtName;
    private TextBox txtPrice;
    private ComboBox cmbUnit;
    private Button btnAdd;
    private Button btnSave;
    private Button btnDelete;

    public IngredientForm(IngredientService ingredientService, UnitService unitService)
    {
        _ingredientService = ingredientService;
        _unitService = unitService;
        InitializeComponent();
        BuildUI();
        LoadUnitsToComboBox();
        LoadIngredients();
    }

    private void BuildUI()
    {
        this.Text = "Ингредиенты";
        this.Size = new Size(600, 450);

        dgvIngredients = new DataGridView
        {
            Location = new Point(10, 10),
            Size = new Size(560, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false
        };

        // лейблы
        var lblName = new Label
        {
            Text = "Название:",
            Location = new Point(10, 275),
            Size = new Size(80, 25)
        };

        txtName = new TextBox
        {
            Location = new Point(100, 272),
            Size = new Size(150, 25),
            PlaceholderText = "Название"
        };

        var lblPrice = new Label
        {
            Text = "Цена:",
            Location = new Point(10, 310),
            Size = new Size(80, 25)
        };

        txtPrice = new TextBox
        {
            Location = new Point(100, 307),
            Size = new Size(150, 25),
            PlaceholderText = "Цена за единицу"
        };

        var lblUnit = new Label
        {
            Text = "Ед. изм.:",
            Location = new Point(10, 345),
            Size = new Size(80, 25)
        };

        cmbUnit = new ComboBox
        {
            Location = new Point(100, 342),
            Size = new Size(150, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        btnAdd = new Button
        {
            Text = "Добавить",
            Location = new Point(270, 272),
            Size = new Size(90, 30)
        };

        btnSave = new Button
        {
            Text = "Сохранить",
            Location = new Point(370, 272),
            Size = new Size(90, 30)
        };

        btnDelete = new Button
        {
            Text = "Удалить",
            Location = new Point(470, 272),
            Size = new Size(90, 30)
        };

        // события
        dgvIngredients.SelectionChanged += dgvIngredients_SelectionChanged;
        btnAdd.Click += btnAdd_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;

        // добавляем на форму
        this.Controls.Add(dgvIngredients);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(lblPrice);
        this.Controls.Add(txtPrice);
        this.Controls.Add(lblUnit);
        this.Controls.Add(cmbUnit);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnDelete);
    }

    private void LoadUnitsToComboBox()
    {
        var units = _unitService.GetAll();
        cmbUnit.DataSource = units;
        cmbUnit.DisplayMember = "Name";
        cmbUnit.ValueMember = "Id";
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
                Name = txtName.Text.Trim(),
                PricePerUnit = double.Parse(txtPrice.Text),
                UnitId = (int)cmbUnit.SelectedValue
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
                Id = _selectedId.Value,
                Name = txtName.Text.Trim(),
                PricePerUnit = double.Parse(txtPrice.Text),
                UnitId = (int)cmbUnit.SelectedValue
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