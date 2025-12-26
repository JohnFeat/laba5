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
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название книги не может быть пустым");
                if (value.Length > 64)
                    throw new ArgumentException("Название книги не может превышать 64 символа");
                _title = value;
            }
        }

        private double _basePrice;
        public double BasePrice
        {
            get => _basePrice;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Цена не может быть отрицательной");
                if (value > 100000)
                    throw new ArgumentException("Цена не может превышать 100 000");
                _basePrice = value;
            }
        }

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
            // Валидация
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название книги не может быть пустым");

            if (title.Length > 64)
                throw new ArgumentException("Название книги не может превышать 64 символа");

            if (basePrice < 0)
                throw new ArgumentException("Цена не может быть отрицательной");

            if (basePrice > 100000)
                throw new ArgumentException("Цена не может превышать 100 000");

            // Проверка уникальности
            if (GetBookByTitle(title) != null)
                throw new ArgumentException("Книга с таким названием уже существует");

            _books.Add(new Book(title, basePrice, strategy));
        }

        public void AddBook(Book book)
        {
            // Проверяем, нет ли уже книги с таким названием
            if (GetBookByTitle(book.Title) != null)
                throw new ArgumentException("Книга с таким названием уже существует");

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

        // Методы для сортировки
        public List<Book> SortByTitle(bool ascending = true)
        {
            return ascending
                ? _books.OrderBy(b => b.Title).ToList()
                : _books.OrderByDescending(b => b.Title).ToList();
        }

        public List<Book> SortByPrice(bool ascending = true)
        {
            return ascending
                ? _books.OrderBy(b => b.BasePrice).ToList()
                : _books.OrderByDescending(b => b.BasePrice).ToList();
        }

        public List<Book> SortByPenalty(bool ascending = true)
        {
            return ascending
                ? _books.OrderBy(b => b.FinalPenalty).ToList()
                : _books.OrderByDescending(b => b.FinalPenalty).ToList();
        }

        public List<Book> SortByBorrowingType(bool ascending = true)
        {
            return ascending
                ? _books.OrderBy(b => b.BorrowingType).ToList()
                : _books.OrderByDescending(b => b.BorrowingType).ToList();
        }

        public List<Book> SortByCreatedDate(bool ascending = true)
        {
            return ascending
                ? _books.OrderBy(b => b.CreatedDate).ToList()
                : _books.OrderByDescending(b => b.CreatedDate).ToList();
        }
    }
}