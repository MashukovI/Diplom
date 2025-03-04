using System.Data.SqlClient;

public class DatabaseService
{
    private string connectionString;

    public DatabaseService(string connectionString)
    {
        this.connectionString = connectionString;
    }

    // Добавляем public getter для connectionString
    public string ConnectionString => connectionString;

    public int CheckUserCredentials(string username, string passwordHash)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT UserId FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Ошибка при проверке учетных данных: " + ex.Message);
        }
    }

    public void RegisterUser(string username, string passwordHash)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                command.ExecuteNonQuery();
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Ошибка при регистрации пользователя: " + ex.Message);
        }
    }

    public void SaveOperation(
    int userId,
    string operationType,
    double width0,
    double stZapKalib,
    double rscrug,
    double koefVit,
    string markSt,
    double temp,
    double result1,
    double result2,
    double result3
)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                INSERT INTO OperationHistory (UserId, OperationType, Width0, StZapKalib, Rscrug, KoefVit, MarkSt, Temp, Result1, Result2, Result3)
                VALUES (@UserId, @OperationType, @Width0, @StZapKalib, @Rscrug, @KoefVit, @MarkSt, @Temp, @Result1, @Result2, @Result3)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@OperationType", operationType);
                command.Parameters.AddWithValue("@Width0", width0);
                command.Parameters.AddWithValue("@StZapKalib", stZapKalib);
                command.Parameters.AddWithValue("@Rscrug", rscrug);
                command.Parameters.AddWithValue("@KoefVit", koefVit);
                command.Parameters.AddWithValue("@MarkSt", markSt);
                command.Parameters.AddWithValue("@Temp", temp);
                command.Parameters.AddWithValue("@Result1", result1);
                command.Parameters.AddWithValue("@Result2", result2);
                command.Parameters.AddWithValue("@Result3", result3);

                command.ExecuteNonQuery();
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Ошибка при сохранении операции: " + ex.Message);
        }
    }

}