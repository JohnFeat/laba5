using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace LibrarySystem
{
    public static class DatabaseService
    {
        private static string connectionString = "Data Source=library.db;Version=3;";

        // Инициализация базы данных
        public static void InitializeDatabase()
        {
            if (!File.Exists("library.db"))
            {
                SQLiteConnection.CreateFile("library.db");
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string sql = @"
                CREATE TABLE IF NOT EXISTS Books (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL UNIQUE,
                    BasePrice REAL NOT NULL,
                    StrategyType TEXT NOT NULL,
                    ExtensionDays INTEGER DEFAULT 0,
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Сохранить все книги
        public static void SaveAllBooks(Library library)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Очищаем таблицу
                using (var cmd = new SQLiteCommand("DELETE FROM Books", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Вставляем все книги
                foreach (var book in library.GetAllBooks())
                {
                    string strategyType = book.Strategy.GetType().Name;
                    int extensionDays = 0;

                    if (book.Strategy is ExtendedBorrowing eb)
                    {
                        extensionDays = eb.DaysExtension;
                    }

                    string sql = @"
                    INSERT INTO Books (Title, BasePrice, StrategyType, ExtensionDays, CreatedDate)
                    VALUES (@title, @price, @strategy, @days, @createdDate)";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", book.Title);
                        cmd.Parameters.AddWithValue("@price", book.BasePrice);
                        cmd.Parameters.AddWithValue("@strategy", strategyType);
                        cmd.Parameters.AddWithValue("@days", extensionDays);
                        cmd.Parameters.AddWithValue("@createdDate", book.CreatedDate);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Загрузить все книги из БД
        public static void LoadAllBooks(Library library)
        {
            library.Clear();

            if (!File.Exists("library.db"))
                return;

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT Title, BasePrice, StrategyType, ExtensionDays, CreatedDate FROM Books";

                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string title = reader.GetString(0);
                        double price = reader.GetDouble(1);
                        string strategyType = reader.GetString(2);
                        int days = reader.GetInt32(3);
                        DateTime createdDate = reader.GetDateTime(4);

                        BorrowingStrategy strategy;

                        if (strategyType == "ExtendedBorrowing" && days > 0)
                        {
                            strategy = new ExtendedBorrowing(days);
                        }
                        else
                        {
                            strategy = new StandardBorrowing();
                        }

                        var book = new Book(title, price, strategy);
                        book.CreatedDate = createdDate;
                        library.AddBook(book);
                    }
                }
            }
        }

        // Добавить одну книгу
        public static void AddBook(Book book)
        {
            // Валидация перед сохранением
            if (book.Title.Length > 64)
                throw new ArgumentException("Название книги слишком длинное для БД");

            if (book.BasePrice > 100000)
                throw new ArgumentException("Слишком большая стоимость для БД");

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string strategyType = book.Strategy.GetType().Name;
                int extensionDays = 0;

                if (book.Strategy is ExtendedBorrowing eb)
                {
                    extensionDays = eb.DaysExtension;
                }

                string sql = @"
                INSERT INTO Books (Title, BasePrice, StrategyType, ExtensionDays, CreatedDate)
                VALUES (@title, @price, @strategy, @days, @createdDate)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@price", book.BasePrice);
                    cmd.Parameters.AddWithValue("@strategy", strategyType);
                    cmd.Parameters.AddWithValue("@days", extensionDays);
                    cmd.Parameters.AddWithValue("@createdDate", book.CreatedDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Обновить книгу
        public static void UpdateBook(string oldTitle, Book book)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string strategyType = book.Strategy.GetType().Name;
                int extensionDays = 0;

                if (book.Strategy is ExtendedBorrowing eb)
                {
                    extensionDays = eb.DaysExtension;
                }

                string sql = @"
                UPDATE Books 
                SET Title = @newTitle, 
                    BasePrice = @price, 
                    StrategyType = @strategy, 
                    ExtensionDays = @days,
                    CreatedDate = @createdDate
                WHERE Title = @oldTitle";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@newTitle", book.Title);
                    cmd.Parameters.AddWithValue("@price", book.BasePrice);
                    cmd.Parameters.AddWithValue("@strategy", strategyType);
                    cmd.Parameters.AddWithValue("@days", extensionDays);
                    cmd.Parameters.AddWithValue("@createdDate", book.CreatedDate);
                    cmd.Parameters.AddWithValue("@oldTitle", oldTitle);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Удалить книгу
        public static void DeleteBook(string title)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Books WHERE Title = @title";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Получить количество книг
        public static int GetBookCount()
        {
            if (!File.Exists("library.db"))
                return 0;

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM Books";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}