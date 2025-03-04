using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EngineeringCalculator
{
    public partial class MainForm : Form
    {
        private int UserId = -1; // �� ��������� ������������ �� �����������
        private bool IsLoggedIn => UserId != -1; // ���� �����������
        private DatabaseService databaseService;
        private CalculatorEngine calculatorEngine;

        public MainForm(DatabaseService databaseService)
        {
            InitializeComponent();
            this.databaseService = databaseService;
            this.calculatorEngine = new CalculatorEngine(); // ������������� ������������
            InitializeComponents();
            UpdateUI(); // ��������� ��������� ��� �������
            UpdateMode(); // ������������� ��������� ���������
        }

        public MainForm(int userId, string username, DatabaseService databaseService) : this(databaseService)
        {
            UserId = userId;
            labelUsername.Text = username; // ������������� ��� ������������
            labelUserName2.Text = username; // ������������� ��� ������������ �� ������ Label
            UpdateUI(); // ��������� ���������
        }

        private void InitializeComponents()
        {
            // ��������� ComboBox ��� ������ ���������
            comboBoxZag.Items.AddRange(new string[] { "�������", "�������������" });
            comboBoxZag.SelectedIndex = 0;
            comboBoxZag.SelectedIndexChanged += comboBoxZag_SelectedIndexChanged;

            // ��������� ComboBox ��� ������ �������
            comboBoxKalib.Items.AddRange(new string[] { "����", "����", "�������" });
            comboBoxKalib.SelectedIndex = 0;
            comboBoxKalib.SelectedIndexChanged += comboBoxKalib_SelectedIndexChanged;

            // ��������� ComboBox ��� ����� �����
            comboBoxMarkSt.Items.AddRange(new string[] { "����� 1", "����� 2", "����� 3" });
            comboBoxMarkSt.SelectedIndex = 0;

            // �������������� ��������� ���������
            UpdateMode();
        }

        private void UpdateControlsVisibility(string mode)
        {
            // �������� ��� �������� �� ���������
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

            // ���������� �������� � ����������� �� ������
            switch (mode)
            {
                case "�������-����":
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

                    // ��������� ����������� ��� ������ "�������-����"
                    pictureBoxMode.Image = Properties.Resources.SquareOval;
                    break;

                case "�������������-�������":
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

                    // ��������� ����������� ��� ������ "�������������-�������"
                    pictureBoxMode.Image = Properties.Resources.HexagonSquare;
                    break;

                case "�������-����":
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

                    // ��������� ����������� ��� ������ "�������-����"
                    pictureBoxMode.Image = Properties.Resources.SquareRhombus;
                    break;

                default:
                    // ���� ����� �� ��������������, ��� �������� ������
                    MessageBox.Show($"����� '{mode}' �� ��������������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // ������� �����������
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
                MessageBox.Show("�������� ��������� � ������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mode = $"{zag}-{kalib}";
            UpdateControlsVisibility(mode);
        }

        private bool IsModeSupported(string mode)
        {
            // ������ �������������� �������
            string[] supportedModes = { "�������-����", "�������������-�������", "�������-����" };
            return supportedModes.Contains(mode);
        }
        private void UpdateUI()
        {
            // ��������� ��������� ������, ����� ������������ � GroupBox
            buttonLogin.Visible = !IsLoggedIn;
            buttonLogout.Visible = IsLoggedIn;
            buttonHistory.Visible = IsLoggedIn; // ���������� ������ ������� ��� �����������
            labelUsername.Visible = IsLoggedIn;
            labelUserName2.Visible = IsLoggedIn;
            groupBox.Visible = IsLoggedIn;
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            // ��������� ������� ������
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

            // �������� ��������� ��������� � ������
            string zag = comboBoxZag.SelectedItem.ToString();
            string kalib = comboBoxKalib.SelectedItem.ToString();
            string mode = $"{zag}-{kalib}";

            try
            {
                // ��������� �������
                calculatorEngine.Calculate(mode, Width0, StZapKalib, Rscrug, KoefVit, Temp, NachDVal);

                // ��������� ���������� � ���� ������, ���� ������������ �����������
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

                // ���������� ����������
                DisplayResults();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ��� ���������� ��������: " + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetInputDataForMode(string mode, double Width0, double StZapKalib, double Rscrug, double KoefVit, string MarkSt, double Temp)
        {
            // ��������� ������ � �������� ������� � ����������� �� ������
            switch (mode)
            {
                case "�������-����":
                    return $"Width0={Width0}, StZapKalib={StZapKalib}, Rscrug={Rscrug}";
                case "�������-����":
                    return $"Width0={Width0}, Temp={Temp}, KoefVit={KoefVit}";
                case "�������������-�������":
                    return $"Rscrug={Rscrug}, KoefVit={KoefVit}, Width0={Width0}, MarkSt={MarkSt}";
                default:
                    throw new ArgumentException("������ ������������ ����� ������.");
            }
        }

        private bool ValidateInputs()
        {
            // ��������� ������ ������� ����
            if (textBoxWidth0.Visible && !double.TryParse(textBoxWidth0.Text, out _))
            {
                MessageBox.Show("����������, ������� ���������� �������� ��� Width0.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxStZapKalib.Visible && !double.TryParse(textBoxStZapKalib.Text, out _))
            {
                MessageBox.Show("����������, ������� ���������� �������� ��� StZapKalib.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxRscrug.Visible && !double.TryParse(textBoxRscrug.Text, out _))
            {
                MessageBox.Show("����������, ������� ���������� �������� ��� Rscrug.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxKoefVit.Visible && !double.TryParse(textBoxKoefVit.Text, out _))
            {
                MessageBox.Show("����������, ������� ���������� �������� ��� KoefVit.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxTemp.Visible && !double.TryParse(textBoxTemp.Text, out _))
            {
                MessageBox.Show("����������, ������� ���������� �������� ��� Temp.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxNachDVal.Visible && !double.TryParse(textBoxNachDVal.Text, out _))
            {
                MessageBox.Show("����������, ������� ���������� �������� ��� NachDVal.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // ��������� ����� �����������
            LoginForm loginForm = new LoginForm(databaseService);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // ���� ����������� �������, ��������� UserId � ��� ������������
                UserId = loginForm.UserId;
                labelUsername.Text = loginForm.Username; // ������������� ��� ������������
                labelUserName2.Text = "������������:"; // ������������� ��� ������������ �� ������ Label
                UpdateUI(); // ��������� ���������
                MessageBox.Show("����������� �������!");
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            // ����� �� ������� ������
            UserId = -1;
            labelUsername.Text = string.Empty; // ������� ��� ������������
            labelUserName2.Text = string.Empty; // ������� ��� ������������ �� ������ Label
            UpdateUI(); // ��������� ���������
            MessageBox.Show("�� ����� �� ������� ������.");
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            // ��������� ����� ������� ��������
            HistoryForm historyForm = new HistoryForm(UserId, databaseService.ConnectionString);
            historyForm.ShowDialog(); // ���������� ����� ��� ��������� ����
        }
    }
}