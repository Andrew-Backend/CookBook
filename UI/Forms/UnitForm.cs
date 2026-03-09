using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class UnitForm : Form
{
    private readonly UnitService _unitService;

    public UnitForm(UnitService unitService)
    {
        InitializeComponent();
        _unitService = unitService;
    }

    private void UnitForm_Load(object sender, EventArgs e)
    {
        LoadUnits();
    }

    private void LoadUnits()
    {
        var units = _unitService.GetAll();
        dgvUnits.DataSource = units;

        // скрываем технические колонки
        dgvUnits.Columns["Id"].Visible = false;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var unit = new UnitIngredients
            {
                NameUnit = txtName.Text.Trim()
            };

            _unitService.Add(unit);
            txtName.Clear();
            LoadUnits();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (dgvUnits.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите строку для удаления", "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var id = (int)dgvUnits.SelectedRows[0].Cells["Id"].Value;

        var confirm = MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
        {
            try
            {
                _unitService.Delete(id);
                LoadUnits();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}