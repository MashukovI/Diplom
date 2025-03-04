using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace EngineeringCalculator
{
    public partial class RegisterForm : Form
    {
        private DatabaseService databaseService;

        public RegisterForm(DatabaseService databaseService)
        {
            InitializeComponent();
            this.databaseService = databaseService;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // Валидация данных
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов.");
                return;
            }

            // Хэшируем пароль
            string passwordHash = PasswordHelper.HashPassword(password);

            try
            {
                // Регистрируем пользователя
                databaseService.RegisterUser(username, passwordHash);
                MessageBox.Show("Регистрация успешна!");
                this.Close(); // Закрываем форму регистрации
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private TextBox textBoxUsername;
        private Label Username;
        private Label label1;
        private Button RegisterButton;
        private TextBox textBoxPassword;

        private void InitializeComponent()
        {
            textBoxUsername = new TextBox();
            Username = new Label();
            label1 = new Label();
            textBoxPassword = new TextBox();
            RegisterButton = new Button();
            SuspendLayout();
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(57, 58);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(100, 23);
            textBoxUsername.TabIndex = 0;
            // 
            // Username
            // 
            Username.AutoSize = true;
            Username.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Username.Location = new Point(57, 34);
            Username.Name = "Username";
            Username.Size = new Size(84, 21);
            Username.TabIndex = 1;
            Username.Text = "Username";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(57, 103);
            label1.Name = "label1";
            label1.Size = new Size(84, 21);
            label1.TabIndex = 3;
            label1.Text = "Password";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(57, 127);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(100, 23);
            textBoxPassword.TabIndex = 2;
            // 
            // RegisterButton
            // 
            RegisterButton.Location = new Point(57, 178);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(100, 23);
            RegisterButton.TabIndex = 4;
            RegisterButton.Text = "Регистрация";
            RegisterButton.UseVisualStyleBackColor = true;
            RegisterButton.Click += RegisterButton_Click;
            // 
            // RegisterForm
            // 
            ClientSize = new Size(209, 278);
            Controls.Add(RegisterButton);
            Controls.Add(label1);
            Controls.Add(textBoxPassword);
            Controls.Add(Username);
            Controls.Add(textBoxUsername);
            Name = "RegisterForm";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

