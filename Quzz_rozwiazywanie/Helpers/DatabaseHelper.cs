using System.Collections.Generic;
using System.Data.SQLite;

namespace Quzz_rozwiazywanie.Helpers
{
    public static class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=C:\\Quizy\\QUIZY.db";

        public static List<string> GetQuizNames()
        {
            List<string> names = new();
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            var cmd = new SQLiteCommand("SELECT QuizName FROM Quizzes", connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                names.Add(reader.GetString(0));

            return names;
        }

        public static string LoadEncryptedQuizJson(string name)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            var cmd = new SQLiteCommand("SELECT EncryptedJson FROM Quizzes WHERE QuizName = @name", connection);
            cmd.Parameters.AddWithValue("@name", name);

            return cmd.ExecuteScalar() as string;
        }
    }
}
