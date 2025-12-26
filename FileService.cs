using System;
using System.IO;
using System.Linq;

namespace LibrarySystem
{
    public static class FileService
    {
        // Экспорт в текстовый файл
        public static void ExportToFile(Library library, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Каталог книг библиотеки");
                writer.WriteLine("Дата экспорта: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                writer.WriteLine(new string('-', 50));

                foreach (var book in library.GetAllBooks())
                {
                    string days = "";
                    if (book.Strategy is ExtendedBorrowing eb)
                    {
                        days = eb.DaysExtension.ToString();
                    }

                    writer.WriteLine($"Название: {book.Title}");
                    writer.WriteLine($"Стоимость: {book.BasePrice:F2} руб.");
                    writer.WriteLine($"Штраф: {book.FinalPenalty:F2} руб.");
                    writer.WriteLine($"Тип выдачи: {book.BorrowingType}");
                    writer.WriteLine(new string('-', 30));
                }
            }
        }

        // Импорт из текстового файла (простой формат)
        public static void ImportFromFile(Library library, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден", filePath);

            var lines = File.ReadAllLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line) && line.Contains("|"))
                .ToList();

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length < 2) continue;

                string title = parts[0].Trim();

                // Проверка длины названия
                if (title.Length > 64)
                {
                    title = title.Substring(0, 64);
                }

                if (!double.TryParse(parts[1].Trim(), out double price))
                    continue;

                // Проверка диапазона цены
                if (price < 0) price = 0;
                if (price > 100000) price = 100000;

                BorrowingStrategy strategy = new StandardBorrowing();

                if (parts.Length >= 3 && int.TryParse(parts[2].Trim(), out int days) && days > 0 && days <= 30)
                {
                    strategy = new ExtendedBorrowing(days);
                }

                try
                {
                    library.AddBook(title, price, strategy);
                }
                catch (ArgumentException ex)
                {
                    // Пропускаем дубликаты и невалидные данные
                    Console.WriteLine($"Ошибка импорта: {ex.Message}");
                }
            }
        }
    }
}