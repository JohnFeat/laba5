using System;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class EditForm : Form
    {
        private readonly Library _library;
        private readonly Book _editingBook;
        private readonly FormMode _mode;

        public enum FormMode
        {
            Add,       // Добавление новой книги
            Edit,      // Редактирование существующей
            View       // Просмотр (только чтение)
        }

        // Конструктор для добавления книги
        public EditForm(Library library)
            : this(library, null, FormMode.Add)
        {
        }

        // Конструктор для редактирования книги
        public EditForm(Library library, Book book)
            : this(library, book, FormMode.Edit)
        {
        }

        // Конструктор для просмотра книги (только чтение)
        public EditForm(Library library, Book book, bool viewOnly)
            : this(library, book, viewOnly ? FormMode.View : FormMode.Edit)
        {
        }

        // Основной конструктор с режимом
        public EditForm(Library library, Book book, FormMode mode)
        {
            InitializeComponent();
            _library = library;
            _editingBook = book;
            _mode = mode;

            InitializeForm();
        }

        private void InitializeForm()
        {
            switch (_mode)
            {
                case FormMode.Add:
                    Text = "Добавить книгу";
                    btnSave.Text = "Добавить";
                    break;

                case FormMode.Edit:
                    Text = "Редактировать книгу";
                    btnSave.Text = "Сохранить";
                    LoadBookData();
                    break;

                case FormMode.View:
                    Text = "Просмотр книги";
                    btnSave.Text = "Закрыть";
                    btnSave.DialogResult = DialogResult.OK; // Закрыть форму при нажатии
                    LoadBookData();
                    SetControlsReadOnly(true);
                    break;
            }
        }

        private void LoadBookData()
        {
            if (_editingBook == null) return;

            txtTitle.Text = _editingBook.Title;
            txtPrice.Text = _editingBook.BasePrice.ToString("F2");

            if (_editingBook.Strategy is ExtendedBorrowing eb)
            {
                chkExtended.Checked = true;
                numDays.Value = eb.DaysExtension;
            }
        }

        private void SetControlsReadOnly(bool readOnly)
        {
            txtTitle.ReadOnly = readOnly;
            txtPrice.ReadOnly = readOnly;
            chkExtended.Enabled = !readOnly;
            numDays.Enabled = !readOnly && chkExtended.Checked;

            // Если режим просмотра, меняем цвет фона для наглядности
            if (readOnly)
            {
                txtTitle.BackColor = System.Drawing.Color.LightGray;
                txtPrice.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                txtTitle.BackColor = System.Drawing.Color.White;
                txtPrice.BackColor = System.Drawing.Color.White;
            }
        }

        // Обработчик изменения текста в названии
        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            // Показываем количество оставшихся символов
            int remaining = 64 - txtTitle.Text.Length;
            if (remaining < 10)
            {
                lblTitle.Text = $"Название книги ({remaining} осталось):";
                if (remaining < 0)
                {
                    txtTitle.Text = txtTitle.Text.Substring(0, 64);
                    txtTitle.SelectionStart = 64;
                }
            }
            else
            {
                lblTitle.Text = "Название книги:";
            }
        }

        // Обработчик ввода в поле цены (только цифры и точка)
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем: цифры, Backspace, Delete, точка (только одна)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Проверяем, что точка только одна
            if (e.KeyChar == '.' && txtPrice.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // Если первая цифра - 0, следующая должна быть точка
            if (txtPrice.Text.Length == 1 && txtPrice.Text[0] == '0' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        // Обработчик изменения текста в поле цены
        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            // Убираем лишние символы
            string text = txtPrice.Text;
            if (string.IsNullOrEmpty(text)) return;

            // Если текст начинается с точки, добавляем 0
            if (text.StartsWith("."))
            {
                txtPrice.Text = "0" + text;
                txtPrice.SelectionStart = txtPrice.Text.Length;
                return;
            }

            // Проверяем значение
            if (double.TryParse(text, out double value))
            {
                if (value > 100000)
                {
                    txtPrice.Text = "100000";
                    txtPrice.SelectionStart = txtPrice.Text.Length;
                    MessageBox.Show("Максимальная стоимость - 100 000 рублей", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (value < 0)
                {
                    txtPrice.Text = "0";
                    txtPrice.SelectionStart = txtPrice.Text.Length;
                }
            }
        }

        private void chkExtended_CheckedChanged(object sender, EventArgs e)
        {
            numDays.Enabled = chkExtended.Checked && _mode != FormMode.View;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // В режиме просмотра просто закрываем форму
                if (_mode == FormMode.View)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                // Валидация названия
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Введите название книги", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
                    return;
                }

                if (txtTitle.Text.Length > 64)
                {
                    MessageBox.Show("Название книги не может превышать 64 символа", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
                    return;
                }

                // Валидация цены
                if (!double.TryParse(txtPrice.Text, out double price))
                {
                    MessageBox.Show("Введите корректную стоимость", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                if (price < 0)
                {
                    MessageBox.Show("Стоимость не может быть отрицательной", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                if (price > 100000)
                {
                    MessageBox.Show("Максимальная стоимость - 100 000 рублей", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                // Проверяем уникальность названия (только при добавлении)
                if (_mode == FormMode.Add && _library.GetBookByTitle(txtTitle.Text.Trim()) != null)
                {
                    MessageBox.Show("Книга с таким названием уже существует", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
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
                if (_mode == FormMode.Add)
                {
                    // Добавление новой книги
                    var newBook = new Book(txtTitle.Text.Trim(), Math.Round(price, 2), strategy);

                    // Сохраняем в БД
                    DatabaseService.AddBook(newBook);

                    // Добавляем в коллекцию
                    _library.AddBook(newBook);
                }
                else if (_mode == FormMode.Edit)
                {
                    // Редактирование существующей
                    string oldTitle = _editingBook.Title;

                    // Проверяем, не меняется ли название на существующее
                    if (oldTitle != txtTitle.Text.Trim() &&
                        _library.GetBookByTitle(txtTitle.Text.Trim()) != null)
                    {
                        MessageBox.Show("Книга с таким названием уже существует", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTitle.Focus();
                        return;
                    }

                    _editingBook.Title = txtTitle.Text.Trim();
                    _editingBook.BasePrice = Math.Round(price, 2);
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