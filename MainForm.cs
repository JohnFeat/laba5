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

            // Добавляем колонки
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                HeaderText = "Название книги",
                Name = "colTitle",
                Width = 200
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BasePrice",
                HeaderText = "Стоимость",
                Name = "colPrice",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FinalPenalty",
                HeaderText = "Штраф",
                Name = "colPenalty",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BorrowingType",
                HeaderText = "Тип выдачи",
                Name = "colType",
                Width = 150
            });
        }

        private void LoadFromDatabase()
        {
            try
            {
                DatabaseService.LoadAllBooks(_library);
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки из БД: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            lblStatus.Text = $"Книг в БД: {dbCount} | В памяти: {memCount}";
        }

        // === ОБРАБОТЧИКИ СОБЫТИЙ ===

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new EditForm(_library))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshGrid();
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