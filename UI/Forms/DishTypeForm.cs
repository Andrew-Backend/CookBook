using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class DishTypeForm : Form
{
    private readonly DishTypeService _dishTypeService;
    private int? _selectedId = null; // null — режим добавления, есть id — режим редактирования

    public DishTypeForm(DishTypeService dishTypeService)
    {
        InitializeComponent();
        _dishTypeService = dishTypeService;
    }

    private void DishTypeForm_Load(object sender, EventArgs e)
    {
        LoadDishTypes();
    }

    private void LoadDishTypes()
    {
        var types = _dishTypeService.GetAll();
        dgvDishTypes.DataSource = types;
        dgvDishTypes.Columns["Id"].Visible = false;
    }

    // при клике на строку — заполняем поле для редактирования
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
            var dishType = new DishType { NameDishType = txtName.Text.Trim() };
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
                IdTypeDish = _selectedId.Value,
                NameDishType = txtName.Text.Trim()
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