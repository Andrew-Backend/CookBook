using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class DishTypeForm : Form
{
    private readonly DishTypeService _dishTypeService;
    private int? _selectedId = null;

    // контролы
    private DataGridView dgvDishTypes;
    private TextBox txtName;
    private Button btnAdd;
    private Button btnSave;
    private Button btnDelete;

    public DishTypeForm(DishTypeService dishTypeService)
    {
        _dishTypeService = dishTypeService;
        InitializeComponent();
        BuildUI();
        LoadDishTypes();
    }

    private void BuildUI()
    {
        this.Text = "Типы блюд";
        this.Size = new Size(500, 400);

        dgvDishTypes = new DataGridView
        {
            Location = new Point(10, 10),
            Size = new Size(460, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false
        };

        txtName = new TextBox
        {
            Location = new Point(10, 270),
            Size = new Size(200, 30),
            PlaceholderText = "Название типа блюда"
        };

        btnAdd = new Button
        {
            Text = "Добавить",
            Location = new Point(220, 268),
            Size = new Size(100, 30)
        };

        btnSave = new Button
        {
            Text = "Сохранить",
            Location = new Point(330, 268),
            Size = new Size(100, 30)
        };

        btnDelete = new Button
        {
            Text = "Удалить",
            Location = new Point(440, 268),
            Size = new Size(100, 30)
        };

        // события
        dgvDishTypes.SelectionChanged += dgvDishTypes_SelectionChanged;
        btnAdd.Click += btnAdd_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;

        this.Controls.Add(dgvDishTypes);
        this.Controls.Add(txtName);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnDelete);
    }

    private void LoadDishTypes()
    {
        var types = _dishTypeService.GetAll();
        dgvDishTypes.DataSource = types;
        dgvDishTypes.Columns["Id"].Visible = false;
    }

    private void dgvDishTypes_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvDishTypes.SelectedRows.Count == 0) return;

        _selectedId = (int)dgvDishTypes.SelectedRows[0].Cells["Id"].Value;
        txtName.Text = (string)dgvDishTypes.SelectedRows[0].Cells["Name"].Value;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var dishType = new DishType { Name = txtName.Text.Trim() };
            _dishTypeService.Add(dishType);
            ClearForm();
            LoadDishTypes();
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
            var dishType = new DishType
            {
                Id = _selectedId.Value,
                Name = txtName.Text.Trim()
            };
            _dishTypeService.Update(dishType);
            ClearForm();
            LoadDishTypes();
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
                _dishTypeService.Delete(_selectedId.Value);
                ClearForm();
                LoadDishTypes();
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
        _selectedId = null;
        dgvDishTypes.ClearSelection();
    }
}