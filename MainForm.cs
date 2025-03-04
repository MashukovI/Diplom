using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EngineeringCalculator
{
    public partial class MainForm : Form
    {
        private int UserId = -1; // По умолчанию пользователь не авторизован
        private bool IsLoggedIn => UserId != -1; // Флаг авторизации
        private DatabaseService databaseService;
        private CalculatorEngine calculatorEngine;

        public MainForm(DatabaseService databaseService)
        {
            InitializeComponent();
            this.databaseService = databaseService;
            this.calculatorEngine = new CalculatorEngine(); // Инициализация калькулятора
            InitializeComponents();
            UpdateUI(); // Обновляем интерфейс при запуске
            UpdateMode(); // Устанавливаем видимость элементов
        }

        public MainForm(int userId, string username, DatabaseService databaseService) : this(databaseService)
        {
            UserId = userId;
            labelUsername.Text = username; // Устанавливаем имя пользователя
            labelUserName2.Text = username; // Устанавливаем имя пользователя во второй Label
            UpdateUI(); // Обновляем интерфейс
        }

        private void InitializeComponents()
        {
            // Заполняем ComboBox для выбора заготовки
            comboBoxZag.Items.AddRange(new string[] { "Квадрат", "Шестиугольник" });
            comboBoxZag.SelectedIndex = 0;
            comboBoxZag.SelectedIndexChanged += comboBoxZag_SelectedIndexChanged;

            // Заполняем ComboBox для выбора калибра
            comboBoxKalib.Items.AddRange(new string[] { "Ромб", "Овал", "Квадрат" });
            comboBoxKalib.SelectedIndex = 0;
            comboBoxKalib.SelectedIndexChanged += comboBoxKalib_SelectedIndexChanged;

            // Заполняем ComboBox для марки стали
            comboBoxMarkSt.Items.AddRange(new string[] { "Сталь 1", "Сталь 2", "Сталь 3" });
            comboBoxMarkSt.SelectedIndex = 0;

            // Инициализируем видимость элементов
            UpdateMode();
        }

        private void UpdateControlsVisibility(string mode)
        {
            // Скрываем все элементы по умолчанию
            labelWidth0.Visible = false;
            textBoxWidth0.Visible = false;
            labelStZapKalib.Visible = false;
            textBoxStZapKalib.Visible = false;
            labelRscrug.Visible = false;
            textBoxRscrug.Visible = false;
            labelKoefVit.Visible = false;
            textBoxKoefVit.Visible = false;
            labelMarkSt.Visible = false;
            comboBoxMarkSt.Visible = false;
            labelTemp.Visible = false;
            textBoxTemp.Visible = false;
            labelNachDVal.Visible = false;
            textBoxNachDVal.Visible = false;

            // Показываем элементы в зависимости от режима
            switch (mode)
            {
                case "Квадрат-Овал":
                    labelWidth0.Visible = true;
                    textBoxWidth0.Visible = true;
                    labelStZapKalib.Visible = true;
                    textBoxStZapKalib.Visible = true;
                    labelRscrug.Visible = true;
                    textBoxRscrug.Visible = true;
                    labelKoefVit.Visible = true;
                    textBoxKoefVit.Visible = true;
                    labelMarkSt.Visible = true;
                    comboBoxMarkSt.Visible = true;
                    labelTemp.Visible = true;
                    textBoxTemp.Visible = true;

                    // Загружаем изображение для режима "Квадрат-Овал"
                    pictureBoxMode.Image = Properties.Resources.SquareOval;
                    break;

                case "Шестиугольник-Квадрат":
                    labelWidth0.Visible = true;
                    textBoxWidth0.Visible = true;
                    labelStZapKalib.Visible = true;
                    textBoxStZapKalib.Visible = true;
                    labelMarkSt.Visible = true;
                    comboBoxMarkSt.Visible = true;
                    labelTemp.Visible = true;
                    textBoxTemp.Visible = true;
                    labelNachDVal.Visible = true;
                    textBoxNachDVal.Visible = true;

                    // Загружаем изображение для режима "Шестиугольник-Квадрат"
                    pictureBoxMode.Image = Properties.Resources.HexagonSquare;
                    break;

                case "Квадрат-Ромб":
                    labelWidth0.Visible = true;
                    textBoxWidth0.Visible = true;
                    labelStZapKalib.Visible = true;
                    textBoxStZapKalib.Visible = true;
                    labelRscrug.Visible = true;
                    textBoxRscrug.Visible = true;
                    labelMarkSt.Visible = true;
                    comboBoxMarkSt.Visible = true;
                    labelTemp.Visible = true;
                    textBoxTemp.Visible = true;

                    // Загружаем изображение для режима "Квадрат-Ромб"
                    pictureBoxMode.Image = Properties.Resources.SquareRhombus;
                    break;

                default:
                    // Если режим не поддерживается, все элементы скрыты
                    MessageBox.Show($"Режим '{mode}' не поддерживается.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Очищаем изображение
                    pictureBoxMode.Image = null;
                    break;
            }
        }

        private void comboBoxZag_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMode();
        }

        private void comboBoxKalib_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMode();
        }

        private void UpdateMode()
        {
            string zag = comboBoxZag.SelectedItem?.ToString();
            string kalib = comboBoxKalib.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(zag) || string.IsNullOrEmpty(kalib))
            {
                MessageBox.Show("Выберите заготовку и калибр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mode = $"{zag}-{kalib}";
            UpdateControlsVisibility(mode);
        }

        private bool IsModeSupported(string mode)
        {
            // Список поддерживаемых режимов
            string[] supportedModes = { "Квадрат-Овал", "Шестиугольник-Квадрат", "Квадрат-Ромб" };
            return supportedModes.Contains(mode);
        }
        private void UpdateUI()
        {
            // Обновляем видимость кнопок, имени пользователя и GroupBox
            buttonLogin.Visible = !IsLoggedIn;
            buttonLogout.Visible = IsLoggedIn;
            buttonHistory.Visible = IsLoggedIn; // Показываем кнопку истории при авторизации
            labelUsername.Visible = IsLoggedIn;
            labelUserName2.Visible = IsLoggedIn;
            groupBox.Visible = IsLoggedIn;
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            // Валидация входных данных
            if (!ValidateInputs())
            {
                return;
            }

            double Width0 = textBoxWidth0.Visible ? double.Parse(textBoxWidth0.Text) : 0;
            double StZapKalib = textBoxStZapKalib.Visible ? double.Parse(textBoxStZapKalib.Text) : 0;
            double Rscrug = textBoxRscrug.Visible ? double.Parse(textBoxRscrug.Text) : 0;
            double KoefVit = textBoxKoefVit.Visible ? double.Parse(textBoxKoefVit.Text) : 0;
            string MarkSt = comboBoxMarkSt.Visible ? comboBoxMarkSt.SelectedItem.ToString() : string.Empty;
            double Temp = textBoxTemp.Visible ? double.Parse(textBoxTemp.Text) : 0;
            double NachDVal = textBoxNachDVal.Visible ? double.Parse(textBoxNachDVal.Text) : 0;

            // Получаем выбранные заготовку и калибр
            string zag = comboBoxZag.SelectedItem.ToString();
            string kalib = comboBoxKalib.SelectedItem.ToString();
            string mode = $"{zag}-{kalib}";

            try
            {
                // Выполняем расчеты
                calculatorEngine.Calculate(mode, Width0, StZapKalib, Rscrug, KoefVit, Temp, NachDVal);

                // Сохраняем результаты в базу данных, если пользователь авторизован
                if (IsLoggedIn)
                {
                    databaseService.SaveOperation(
                        UserId,          
                        mode,           
                        Width0,         
                        StZapKalib,     
                        Rscrug,         
                        KoefVit,        
                        MarkSt,         
                        Temp,           
                        calculatorEngine.Result1, 
                        calculatorEngine.Result2,
                        calculatorEngine.Result3  
                    );
                }

                // Отображаем результаты
                DisplayResults();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении расчетов: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetInputDataForMode(string mode, double Width0, double StZapKalib, double Rscrug, double KoefVit, string MarkSt, double Temp)
        {
            // Формируем строку с входными данными в зависимости от режима
            switch (mode)
            {
                case "Квадрат-Ромб":
                    return $"Width0={Width0}, StZapKalib={StZapKalib}, Rscrug={Rscrug}";
                case "Квадрат-Овал":
                    return $"Width0={Width0}, Temp={Temp}, KoefVit={KoefVit}";
                case "Шестиугольник-Квадрат":
                    return $"Rscrug={Rscrug}, KoefVit={KoefVit}, Width0={Width0}, MarkSt={MarkSt}";
                default:
                    throw new ArgumentException("Выбран недопустимый режим работы.");
            }
        }

        private bool ValidateInputs()
        {
            // Проверяем только видимые поля
            if (textBoxWidth0.Visible && !double.TryParse(textBoxWidth0.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для Width0.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxStZapKalib.Visible && !double.TryParse(textBoxStZapKalib.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для StZapKalib.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxRscrug.Visible && !double.TryParse(textBoxRscrug.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для Rscrug.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxKoefVit.Visible && !double.TryParse(textBoxKoefVit.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для KoefVit.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxTemp.Visible && !double.TryParse(textBoxTemp.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для Temp.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxNachDVal.Visible && !double.TryParse(textBoxNachDVal.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для NachDVal.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void DisplayResults()
        {
            resultTextBox1.Text = calculatorEngine.Result1.ToString();
            resultTextBox2.Text = calculatorEngine.Result2.ToString();
            resultTextBox3.Text = calculatorEngine.Result3.ToString();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Открываем форму авторизации
            LoginForm loginForm = new LoginForm(databaseService);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Если авторизация успешна, обновляем UserId и имя пользователя
                UserId = loginForm.UserId;
                labelUsername.Text = loginForm.Username; // Устанавливаем имя пользователя
                labelUserName2.Text = "Пользователь:"; // Устанавливаем имя пользователя во второй Label
                UpdateUI(); // Обновляем интерфейс
                MessageBox.Show("Авторизация успешна!");
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            // Выход из учетной записи
            UserId = -1;
            labelUsername.Text = string.Empty; // Очищаем имя пользователя
            labelUserName2.Text = string.Empty; // Очищаем имя пользователя во второй Label
            UpdateUI(); // Обновляем интерфейс
            MessageBox.Show("Вы вышли из учетной записи.");
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            // Открываем форму истории операций
            HistoryForm historyForm = new HistoryForm(UserId, databaseService.ConnectionString);
            historyForm.ShowDialog(); // Показываем форму как модальное окно
        }
    }
}