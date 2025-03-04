using EngineeringCalculator.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EngineeringCalculator
{
    public partial class LoginForm : Form
    {
        public int UserId { get; private set; } = -1; // ID пользователя после успешной авторизации
        public string Username { get; private set; } // Имя пользователя
        private DatabaseService databaseService;

        public LoginForm(DatabaseService databaseService)
        {
            InitializeComponent();
            this.databaseService = databaseService;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // Валидация данных
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            // Хэшируем пароль
            string passwordHash = PasswordHelper.HashPassword(password);

            try
            {
                // Проверяем учетные данные
                UserId = databaseService.CheckUserCredentials(username, passwordHash);
                if (UserId > 0)
                {
                    Username = username; // Сохраняем имя пользователя
                    DialogResult = DialogResult.OK; // Указываем, что авторизация успешна
                    this.Close(); // Закрываем форму
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            // Открываем форму регистрации
            RegisterForm registerForm = new RegisterForm(databaseService);
            registerForm.ShowDialog(); // Показываем форму как модальное окно
        }
        private Label label1;
        private TextBox textBoxPassword;
        private Label UsernameLog;
        private Button buttonRegister;
        private Button LoginButton;
        private TextBox textBoxUsername;

        private void InitializeComponent()
        {
            label1 = new Label();
            textBoxPassword = new TextBox();
            UsernameLog = new Label();
            textBoxUsername = new TextBox();
            buttonRegister = new Button();
            LoginButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(48, 76);
            label1.Name = "label1";
            label1.Size = new Size(84, 21);
            label1.TabIndex = 7;
            label1.Text = "Password";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(48, 100);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(100, 23);
            textBoxPassword.TabIndex = 6;
            // 
            // UsernameLog
            // 
            UsernameLog.AutoSize = true;
            UsernameLog.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            UsernameLog.Location = new Point(48, 9);
            UsernameLog.Name = "UsernameLog";
            UsernameLog.Size = new Size(84, 21);
            UsernameLog.TabIndex = 5;
            UsernameLog.Text = "Username";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(48, 33);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(100, 23);
            textBoxUsername.TabIndex = 4;
            // 
            // buttonRegister
            // 
            buttonRegister.Location = new Point(48, 170);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(100, 23);
            buttonRegister.TabIndex = 8;
            buttonRegister.Text = "Регистрация";
            buttonRegister.UseVisualStyleBackColor = true;
            buttonRegister.Click += buttonRegister_Click;
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(48, 141);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(100, 23);
            LoginButton.TabIndex = 9;
            LoginButton.Text = "Войти";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // LoginForm
            // 
            ClientSize = new Size(188, 244);
            Controls.Add(LoginButton);
            Controls.Add(buttonRegister);
            Controls.Add(label1);
            Controls.Add(textBoxPassword);
            Controls.Add(UsernameLog);
            Controls.Add(textBoxUsername);
            Name = "LoginForm";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}