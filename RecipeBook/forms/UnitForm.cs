using BLL;
using Entity;

namespace Recipe_Book.Forms;

public partial class UnitForm : Form
{
    private readonly UnitService _unitService;

    // контролы
    private DataGridView dgvUnits;
    private TextBox txtName;
    private Button btnAdd;
    private Button btnDelete;

    public UnitForm(UnitService unitService)
    {
        _unitService = unitService;
        InitializeComponent();
        BuildUI();
        LoadUnits();
    }

    private void BuildUI()
    {
        this.Text = "Единицы измерения";
        this.Size = new Size(500, 400);

        // DataGridView
        dgvUnits = new DataGridView
        {
            Location = new Point(10, 10),
            Size = new Size(460, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false
        };

        // TextBox
        txtName = new TextBox
        {
            Location = new Point(10, 270),
            Size = new Size(200, 30),
            PlaceholderText = "Название единицы измерения"
        };

        // кнопки
        btnAdd = new Button
        {
            Text = "Добавить",
            Location = new Point(220, 268),
            Size = new Size(100, 30)
        };

        btnDelete = new Button
        {
            Text = "Удалить",
            Location = new Point(330, 268),
            Size = new Size(100, 30)
        };

        // события
        btnAdd.Click += btnAdd_Click;
        btnDelete.Click += btnDelete_Click;

        // добавляем на форму
        this.Controls.Add(dgvUnits);
        this.Controls.Add(txtName);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnDelete);
    }

    private void LoadUnits()
    {
        var units = _unitService.GetAll();
        dgvUnits.DataSource = units;
        dgvUnits.Columns["Id"].Visible = false;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var unit = new UnitIngredients { Name = txtName.Text.Trim() };
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
