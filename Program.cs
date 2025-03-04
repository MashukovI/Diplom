using System;
using System.Data.SqlClient;

namespace EngineeringCalculator
{
    class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string connectionString = "Server=localhost;Database=CalculatorDB;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Подключение к базе данных успешно!");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Ошибка подключения: " + ex.Message);
            }
            // Создаем экземпляр DatabaseService
            DatabaseService databaseService = new DatabaseService(connectionString);

            // Запускаем главную форму
            Application.Run(new MainForm(databaseService));
        }
    }
}