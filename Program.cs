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
                    Console.WriteLine("����������� � ���� ������ �������!");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("������ �����������: " + ex.Message);
            }
            // ������� ��������� DatabaseService
            DatabaseService databaseService = new DatabaseService(connectionString);

            // ��������� ������� �����
            Application.Run(new MainForm(databaseService));
        }
    }
}