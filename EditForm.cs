using System;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class EditForm : Form
    {
        private readonly Library _library;
        private readonly Book _editingBook;

        public EditForm(Library library)
        {
            InitializeComponent();
            _library = library;
            Text = "Добавить книгу";
        }

        public EditForm(Library library, Book book)
        {
            InitializeComponent();
            _library = library;
            _editingBook = book;
            Text = "Редактировать книгу";

            // Заполняем поля
            txtTitle.Text = book.Title;
            txtPrice.Text = book.BasePrice.ToString();

            if (book.Strategy is ExtendedBorrowing eb)
            {
                chkExtended.Checked = true;
                numDays.Value = eb.DaysExtension;
            }
        }

        private void chkExtended_CheckedChanged(object sender, EventArgs e)
        {
            numDays.Enabled = chkExtended.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Введите название книги", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!double.TryParse(txtPrice.Text, out double price) || price < 0)
                {
                    MessageBox.Show("Введите корректную стоимость", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Создаем стратегию
                BorrowingStrategy strategy;
                if (chkExtended.Checked && numDays.Value > 0)
                {
                    strategy = new ExtendedBorrowing((int)numDays.Value);
                }
                else
                {
                    strategy = new StandardBorrowing();
                }

                // Создаем или обновляем книгу
                if (_editingBook == null)
                {
                    // Добавление новой книги
                    var newBook = new Book(txtTitle.Text.Trim(), price, strategy);

                    // Сохраняем в БД
                    DatabaseService.AddBook(newBook);

                    // Добавляем в коллекцию
                    _library.AddBook(newBook);
                }
                else
                {
                    // Редактирование существующей
                    string oldTitle = _editingBook.Title;

                    _editingBook.Title = txtTitle.Text.Trim();
                    _editingBook.BasePrice = price;
                    _editingBook.SetStrategy(strategy);

                    // Обновляем в БД
                    DatabaseService.UpdateBook(oldTitle, _editingBook);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}   