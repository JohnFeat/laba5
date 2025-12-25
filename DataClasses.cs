using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem
{
    // Стратегия выдачи книг
    public abstract class BorrowingStrategy
    {
        public abstract double CalculatePenalty(double basePenalty);
        public abstract string GetStrategyType();
    }

    // Стандартная выдача
    public class StandardBorrowing : BorrowingStrategy
    {
        public override double CalculatePenalty(double basePenalty) => basePenalty;
        public override string GetStrategyType() => "Стандартная";
    }

    // Продленная выдача
    public class ExtendedBorrowing : BorrowingStrategy
    {
        public int DaysExtension { get; set; }

        public ExtendedBorrowing(int days)
        {
            if (days < 0 || days > 30)
                throw new ArgumentException("Продление должно быть от 0 до 30 дней");
            DaysExtension = days;
        }

        public override double CalculatePenalty(double basePenalty)
        {
            double discount = Math.Min(DaysExtension * 0.05, 0.5); // 5% за день, максимум 50%
            return basePenalty * (1 - discount);
        }

        public override string GetStrategyType() => $"Продлено на {DaysExtension} дней";
    }

    // Класс Книга
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double BasePrice { get; set; }
        public BorrowingStrategy Strategy { get; private set; }
        public DateTime CreatedDate { get; set; }

        public Book(string title, double basePrice, BorrowingStrategy strategy)
        {
            Title = title;
            BasePrice = basePrice;
            SetStrategy(strategy);
            CreatedDate = DateTime.Now;
        }

        public void SetStrategy(BorrowingStrategy strategy)
        {
            Strategy = strategy ?? new StandardBorrowing();
        }

        public double FinalPenalty => Strategy.CalculatePenalty(BasePrice);
        public string BorrowingType => Strategy.GetStrategyType();
    }

    // Библиотека (коллекция книг)
    public class Library
    {
        private readonly List<Book> _books = new List<Book>();

        public void AddBook(string title, double basePrice)
        {
            AddBook(title, basePrice, new StandardBorrowing());
        }

        public void AddBook(string title, double basePrice, int extensionDays)
        {
            AddBook(title, basePrice, new ExtendedBorrowing(extensionDays));
        }

        public void AddBook(string title, double basePrice, BorrowingStrategy strategy)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым");

            if (basePrice < 0)
                throw new ArgumentException("Цена не может быть отрицательной");

            _books.Add(new Book(title, basePrice, strategy));
        }

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public List<Book> GetAllBooks() => _books;

        public void RemoveBook(Book book)
        {
            _books.Remove(book);
        }

        public void Clear()
        {
            _books.Clear();
        }

        public List<string> FindBooksWithMinPenalty()
        {
            if (_books.Count == 0)
                throw new InvalidOperationException("Каталог пуст");

            double minPenalty = _books.Min(b => b.FinalPenalty);
            return _books
                .Where(b => Math.Abs(b.FinalPenalty - minPenalty) < 0.01)
                .Select(b => b.Title)
                .ToList();
        }

        public Book GetBookByTitle(string title)
        {
            return _books.FirstOrDefault(b =>
                b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
    }
}       