using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class MainForm : Form
    {
        private Library _library = new Library();
        private BindingSource _bindingSource = new BindingSource();
        private SortOrder _currentSortOrder = SortOrder.None;
        private string _currentSortColumn = "";

        public MainForm()
        {
            InitializeComponent();

            // Инициализация БД
            DatabaseService.InitializeDatabase();

            // Настройка DataGridView
            SetupDataGridView();

            // Загрузка данных из БД
            LoadFromDatabase();

            // Обновляем статус
            UpdateStatus();
        }

        private void SetupDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = _bindingSource;

            // Очищаем существующие колонки
            dataGridView.Columns.Clear();

            // Добавляем колонки
            DataGridViewTextBoxColumn titleColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                HeaderText = "Название книги",
                Name = "colTitle",
                Width = 200,
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BasePrice",
                HeaderText = "Стоимость",
                Name = "colPrice",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            };

            DataGridViewTextBoxColumn penaltyColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FinalPenalty",
                HeaderText = "Штраф",
                Name = "colPenalty",
                Width = 100,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            };

            DataGridViewTextBoxColumn typeColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BorrowingType",
                HeaderText = "Тип выдачи",
                Name = "colType",
                Width = 150,
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            DataGridViewTextBoxColumn createdColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CreatedDate",
                HeaderText = "Дата создания",
                Name = "colCreated",
                Width = 120,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy HH:mm" }
            };

            // Добавляем колонки
            dataGridView.Columns.AddRange(titleColumn, priceColumn, penaltyColumn, typeColumn, createdColumn);

            // Настраиваем сортировку
            dataGridView.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;

            // Настраиваем двойной клик для просмотра
            dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;
        }

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            string columnName = dataGridView.Columns[e.ColumnIndex].Name;
            string propertyName = dataGridView.Columns[e.ColumnIndex].DataPropertyName;

            // Если кликаем по той же колонке, меняем направление сортировки
            if (_currentSortColumn == propertyName)
            {
                _currentSortOrder = _currentSortOrder == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                _currentSortColumn = propertyName;
                _currentSortOrder = SortOrder.Ascending;
            }

            // Сортируем данные
            SortData(propertyName, _currentSortOrder);

            // Обновляем иконки сортировки
            UpdateSortGlyphs(e.ColumnIndex);
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Получаем книгу по двойному клику
            var book = (Book)dataGridView.Rows[e.RowIndex].DataBoundItem;

            // Открываем в режиме просмотра
            using (var form = new EditForm(_library, book, true))
            {
                form.ShowDialog();
            }
        }

        private void SortData(string propertyName, SortOrder sortOrder)
        {
            var books = _library.GetAllBooks();
            IOrderedEnumerable<Book> sortedBooks = null;

            switch (propertyName)
            {
                case "Title":
                    sortedBooks = sortOrder == SortOrder.Ascending
                        ? books.OrderBy(b => b.Title)
                        : books.OrderByDescending(b => b.Title);
                    break;

                case "BasePrice":
                    sortedBooks = sortOrder == SortOrder.Ascending
                        ? books.OrderBy(b => b.BasePrice)
                        : books.OrderByDescending(b => b.BasePrice);
                    break;

                case "FinalPenalty":
                    sortedBooks = sortOrder == SortOrder.Ascending
                        ? books.OrderBy(b => b.FinalPenalty)
                        : books.OrderByDescending(b => b.FinalPenalty);
                    break;

                case "BorrowingType":
                    sortedBooks = sortOrder == SortOrder.Ascending
                        ? books.OrderBy(b => b.BorrowingType)
                        : books.OrderByDescending(b => b.BorrowingType);
                    break;

                case "CreatedDate":
                    sortedBooks = sortOrder == SortOrder.Ascending
                        ? books.OrderBy(b => b.CreatedDate)
                        : books.OrderByDescending(b => b.CreatedDate);
                    break;

                default:
                    return;
            }

            // Применяем сортировку к BindingSource
            _bindingSource.DataSource = null;
            _bindingSource.DataSource = sortedBooks.ToList();
        }

        private void UpdateSortGlyphs(int sortedColumnIndex)
        {
            // Сбрасываем все иконки
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            // Устанавливаем иконку для текущей колонки
            if (sortedColumnIndex >= 0 && sortedColumnIndex < dataGridView.Columns.Count)
            {
                dataGridView.Columns[sortedColumnIndex].HeaderCell.SortGlyphDirection = _currentSortOrder;
            }
        }

        private void LoadFromDatabase()
        {
            try
            {
                DatabaseService.LoadAllBooks(_library);
                RefreshGrid();

                // Сбрасываем сортировку
                _currentSortColumn = "";
                _currentSortOrder = SortOrder.None;
                ResetSortGlyphs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки из БД: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetSortGlyphs()
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            _bindingSource.DataSource = _library.GetAllBooks();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            int dbCount = DatabaseService.GetBookCount();
            int memCount = _library.GetAllBooks().Count;
            string sortInfo = _currentSortColumn != ""
                ? $" | Сортировка: {GetColumnDisplayName(_currentSortColumn)} {(_currentSortOrder == SortOrder.Ascending ? "↑" : "↓")}"
                : "";

            lblStatus.Text = $"Книг в БД: {dbCount} | В памяти: {memCount}{sortInfo}";
        }

        private string GetColumnDisplayName(string propertyName)
        {
            switch (propertyName)
            {
                case "Title": return "Название";
                case "BasePrice": return "Стоимость";
                case "FinalPenalty": return "Штраф";
                case "BorrowingType": return "Тип выдачи";
                case "CreatedDate": return "Дата создания";
                default: return propertyName;
            }
        }

        // === ОБРАБОТЧИКИ СОБЫТИЙ ===

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new EditForm(_library))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshGrid();
                    ResetSortGlyphs();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для редактирования", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var book = (Book)dataGridView.SelectedRows[0].DataBoundItem;

            using (var form = new EditForm(_library, book))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshGrid();
                    // Восстанавливаем сортировку после редактирования
                    if (_currentSortColumn != "")
                    {
                        SortData(_currentSortColumn, _currentSortOrder);
                        UpdateStatus();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для удаления", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var book = (Book)dataGridView.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show($"Удалить книгу '{book.Title}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Удаляем из БД
                    DatabaseService.DeleteBook(book.Title);

                    // Удаляем из памяти
                    _library.RemoveBook(book);

                    RefreshGrid();
                    // Восстанавливаем сортировку после удаления
                    if (_currentSortColumn != "")
                    {
                        SortData(_currentSortColumn, _currentSortOrder);
                        UpdateStatus();
                    }

                    MessageBox.Show("Книга удалена", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для просмотра", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var book = (Book)dataGridView.SelectedRows[0].DataBoundItem;

            // Используем перегрузку конструктора с bool параметром (true = режим просмотра)
            using (var form = new EditForm(_library, book, true))
            {
                form.ShowDialog();  // Форма откроется в режиме просмотра
            }

            // Альтернативный вариант - использование enum:
            // using (var form = new EditForm(_library, book, EditForm.FormMode.View))
            // {
            //     form.ShowDialog();
            // }
        }

        private void btnFindMin_Click(object sender, EventArgs e)
        {
            try
            {
                var minBooks = _library.FindBooksWithMinPenalty();

                if (minBooks.Count == 0)
                {
                    MessageBox.Show("Каталог пуст", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string message;
                if (minBooks.Count == 1)
                {
                    message = $"Книга с минимальным штрафом:\n{minBooks[0]}";
                }
                else
                {
                    message = $"Книги с минимальным штрафом ({minBooks.Count}):\n" +
                              string.Join("\n", minBooks);
                }

                MessageBox.Show(message, "Результат поиска",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                dialog.Title = "Экспорт каталога";
                dialog.DefaultExt = "txt";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileService.ExportToFile(_library, dialog.FileName);
                        MessageBox.Show("Каталог экспортирован", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                dialog.Title = "Импорт каталога";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileService.ImportFromFile(_library, dialog.FileName);

                        // Сохраняем импортированные данные в БД
                        DatabaseService.SaveAllBooks(_library);

                        RefreshGrid();
                        ResetSortGlyphs();

                        MessageBox.Show("Данные импортированы", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadFromDatabase();
            MessageBox.Show("Данные обновлены из базы", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClearDb_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистить всю базу данных?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Очищаем БД
                    using (var conn = new System.Data.SQLite.SQLiteConnection(
                        "Data Source=library.db;Version=3;"))
                    {
                        conn.Open();
                        using (var cmd = new System.Data.SQLite.SQLiteCommand(
                            "DELETE FROM Books", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Очищаем память
                    _library.Clear();
                    RefreshGrid();
                    ResetSortGlyphs();

                    MessageBox.Show("База данных очищена", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSortReset_Click(object sender, EventArgs e)
        {
            _currentSortColumn = "";
            _currentSortOrder = SortOrder.None;
            RefreshGrid();
            ResetSortGlyphs();
            MessageBox.Show("Сортировка сброшена", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Автосохранение при закрытии
            try
            {
                DatabaseService.SaveAllBooks(_library);
            }
            catch
            {
                // Игнорируем ошибки при закрытии
            }
        }
    }
}