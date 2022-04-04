using MySql.Data.MySqlClient;

namespace GrpcServer.Database
{
    public class DatabaseHelper
    {
        private readonly string USER_TABLE = "tbl_user";
        private readonly string USER_ID = "ID";
        private readonly string USER_NAME = "Name";
        private readonly string USER_EMAIL = "Email";
        private readonly string USER_PASSWORD = "Password";

        private readonly MySqlConnection databaseConnection;

        private readonly string connectionString = string.Format("server={0};uid={1};pwd={2};database={3}", "localhost", "root", "", "grpc_test");

        public DatabaseHelper()
        {
            databaseConnection = new MySqlConnection(connectionString);
        }

        public bool InsertUserRecord(UserModel user)
        {
            int result = -1;

            databaseConnection.Open();

            MySqlCommand command = databaseConnection.CreateCommand();

            command.Parameters.AddWithValue("@name", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(user.Password));

            command.CommandText = string.Format("INSERT INTO {0} ({1}, {2}, {3}) VALUES (@name, @email, @password)", USER_TABLE, USER_NAME, USER_EMAIL, USER_PASSWORD);

            try
            {
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }

            return result > 0;
        }

        public int FindOneUser(UserModel user)
        {
            int userID = -1;

            databaseConnection.Open();

            MySqlCommand command = databaseConnection.CreateCommand();

            command.Parameters.AddWithValue("@email", user.Email);

            command.CommandText = string.Format("SELECT * FROM {0} WHERE {1}=@email LIMIT 1", USER_TABLE, USER_EMAIL);

            try
            {
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read() && BCrypt.Net.BCrypt.Verify(user.Password, reader[USER_PASSWORD].ToString()))
                {
                    userID = Convert.ToInt32(reader[USER_ID]);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                databaseConnection.Close();
            }

            return userID;
        }
    }
}
