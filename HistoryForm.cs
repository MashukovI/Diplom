using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace EngineeringCalculator
{
    public partial class HistoryForm : Form
    {
        private int UserId;
        private string connectionString;
        private DataGridView dataGridViewHistory;
        private ComboBox comboBoxOperationType;
        private DateTimePicker dateTimePickerStart;
        private DateTimePicker dateTimePickerEnd;
        private Button buttonApplyFilters;
        private Button buttonDeleteSelected;

        public HistoryForm(int userId, string connectionString)
        {
            InitializeComponent();
            this.UserId = userId;
            this.connectionString = connectionString;

            // Устанавливаем размер окна
            this.Size = new Size(1000, 600); // Ширина: 1000, Высота: 600

            InitializeComponents();
            LoadHistory();
        }

        private void InitializeComponents()
        {
            // Инициализация DataGridView
            dataGridViewHistory = new DataGridView();
            dataGridViewHistory.Dock = DockStyle.Fill;
            dataGridViewHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Автоматическое заполнение столбцов
            dataGridViewHistory.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Включаем перенос текста
            dataGridViewHistory.RowTemplate.Height = 40; // Высота строки
            dataGridViewHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Автоматическое изменение высоты строк
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False; // Заголовки без переноса
            dataGridViewHistory.ReadOnly = true;
            dataGridViewHistory.AllowUserToAddRows = false;
            dataGridViewHistory.AllowUserToDeleteRows = false;
            dataGridViewHistory.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridViewHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewHistory.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dataGridViewHistory.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Включаем сортировку по столбцам
            foreach (DataGridViewColumn column in dataGridViewHistory.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            // Инициализация фильтров
            comboBoxOperationType = new ComboBox();
            comboBoxOperationType.Dock = DockStyle.Top;
            comboBoxOperationType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOperationType.Items.AddRange(new string[] { "Квадрат-Ромб", "Квадрат-Овал", "Шестиугольник-Квадрат" });
            comboBoxOperationType.SelectedIndex = 0;

            dateTimePickerStart = new DateTimePicker();
            dateTimePickerStart.Dock = DockStyle.Top;
            dateTimePickerStart.Format = DateTimePickerFormat.Short;

            dateTimePickerEnd = new DateTimePicker();
            dateTimePickerEnd.Dock = DockStyle.Top;
            dateTimePickerEnd.Format = DateTimePickerFormat.Short;

            buttonApplyFilters = new Button();
            buttonApplyFilters.Dock = DockStyle.Top;
            buttonApplyFilters.Text = "Применить фильтры";
            buttonApplyFilters.Click += (s, e) => ApplyFilters();

            buttonDeleteSelected = new Button();
            buttonDeleteSelected.Dock = DockStyle.Bottom;
            buttonDeleteSelected.Text = "Удалить выбранную запись";
            buttonDeleteSelected.Click += (s, e) => DeleteSelectedRecord();

            // Добавляем элементы управления на форму
            Controls.Add(dataGridViewHistory);
            Controls.Add(buttonApplyFilters);
            Controls.Add(dateTimePickerEnd);
            Controls.Add(dateTimePickerStart);
            Controls.Add(comboBoxOperationType);
            Controls.Add(buttonDeleteSelected);
        }

        private void LoadHistory()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT 
                        Id,
                        OperationType,
                        Width0,
                        StZapKalib,
                        Rscrug,
                        KoefVit,
                        MarkSt,
                        Temp,
                        Result1,
                        Result2,
                        Result3,
                        CalculationDate
                    FROM OperationHistory
                    WHERE UserId = @UserId
                    ORDER BY CalculationDate DESC";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", UserId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Привязываем данные к DataGridView
                    dataGridViewHistory.DataSource = dataTable;

                    // Скрываем первые два столбца
                    if (dataGridViewHistory.Columns.Count >= 2)
                    {
                        dataGridViewHistory.Columns[0].Visible = false; // Скрываем первый столбец (Id)
                        dataGridViewHistory.Columns[1].Visible = false; // Скрываем второй столбец (OperationType)
                    }

                    RestoreColumnSettings();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка при загрузке истории: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveColumnSettings()
        {
            // Сохраняем настройки столбцов
            foreach (DataGridViewColumn column in dataGridViewHistory.Columns)
            {
                column.Tag = column.HeaderText; // Сохраняем заголовок
            }
        }

        private void RestoreColumnSettings()
        {
            // Восстанавливаем настройки столбцов
            if (dataGridViewHistory.Columns.Contains("OperationType"))
            {
                dataGridViewHistory.Columns["OperationType"].HeaderText = "Тип операции";
            }
            if (dataGridViewHistory.Columns.Contains("Width0"))
            {
                dataGridViewHistory.Columns["Width0"].HeaderText = "Высота заготовки";
            }
            if (dataGridViewHistory.Columns.Contains("StZapKalib"))
            {
                dataGridViewHistory.Columns["StZapKalib"].HeaderText = "Степень заполнения";
            }
            if (dataGridViewHistory.Columns.Contains("Rscrug"))
            {
                dataGridViewHistory.Columns["Rscrug"].HeaderText = "Радиус скругления";
            }
            if (dataGridViewHistory.Columns.Contains("KoefVit"))
            {
                dataGridViewHistory.Columns["KoefVit"].HeaderText = "Коэффициент витка";
            }
            if (dataGridViewHistory.Columns.Contains("MarkSt"))
            {
                dataGridViewHistory.Columns["MarkSt"].HeaderText = "Марка стали";
            }
            if (dataGridViewHistory.Columns.Contains("Temp"))
            {
                dataGridViewHistory.Columns["Temp"].HeaderText = "Температура";
            }
            if (dataGridViewHistory.Columns.Contains("Result1"))
            {
                dataGridViewHistory.Columns["Result1"].HeaderText = "Результат 1";
            }
            if (dataGridViewHistory.Columns.Contains("Result2"))
            {
                dataGridViewHistory.Columns["Result2"].HeaderText = "Результат 2";
            }
            if (dataGridViewHistory.Columns.Contains("Result3"))
            {
                dataGridViewHistory.Columns["Result3"].HeaderText = "Результат 3";
            }
            if (dataGridViewHistory.Columns.Contains("CalculationDate"))
            {
                dataGridViewHistory.Columns["CalculationDate"].HeaderText = "Дата расчета";
                dataGridViewHistory.Columns["CalculationDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
            }
        }

        private void ApplyFilters()
        {
            SaveColumnSettings(); // Сохраняем настройки столбцов

            string operationType = comboBoxOperationType.SelectedItem?.ToString();
            DateTime startDate = dateTimePickerStart.Value;
            DateTime endDate = dateTimePickerEnd.Value;

            string query = @"
                SELECT 
                    Id,
                    OperationType,
                    Width0,
                    StZapKalib,
                    Rscrug,
                    KoefVit,
                    MarkSt,
                    Temp,
                    Result1,
                    Result2,
                    Result3,
                    CalculationDate
                FROM OperationHistory
                WHERE UserId = @UserId
                AND (@OperationType IS NULL OR OperationType = @OperationType)
                AND (@StartDate IS NULL OR CalculationDate >= @StartDate)
                AND (@EndDate IS NULL OR CalculationDate <= @EndDate)
                ORDER BY CalculationDate DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@OperationType", string.IsNullOrEmpty(operationType) ? (object)DBNull.Value : operationType);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Привязываем данные к DataGridView
                dataGridViewHistory.DataSource = dataTable;

                // Скрываем первые два столбца
                if (dataGridViewHistory.Columns.Count >= 2)
                {
                    dataGridViewHistory.Columns[0].Visible = false; // Скрываем первый столбец (Id)
                    dataGridViewHistory.Columns[1].Visible = false; // Скрываем второй столбец (OperationType)
                }

                RestoreColumnSettings(); // Восстанавливаем настройки столбцов
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // HistoryForm
            // 
            ClientSize = new Size(284, 261);
            Name = "HistoryForm";
            ResumeLayout(false);
        }

        private void DeleteSelectedRecord()
        {
            if (dataGridViewHistory.SelectedRows.Count > 0)
            {
                // Получаем Id выбранной строки
                int selectedId = Convert.ToInt32(dataGridViewHistory.SelectedRows[0].Cells["Id"].Value);

                // Удаляем запись из базы данных
                string query = "DELETE FROM OperationHistory WHERE Id = @Id";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", selectedId);
                    command.ExecuteNonQuery();
                }

                // Обновляем данные
                LoadHistory();
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}