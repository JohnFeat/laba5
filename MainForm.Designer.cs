using System.Windows.Forms;

namespace LibrarySystem
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnFindMin;
        private Button btnExport;
        private Button btnImport;
        private Button btnRefresh;
        private Button btnClearDb;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridView = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnFindMin = new Button();
            this.btnExport = new Button();
            this.btnImport = new Button();
            this.btnRefresh = new Button();
            this.btnClearDb = new Button();
            this.statusStrip = new StatusStrip();
            this.lblStatus = new ToolStripStatusLabel();

            this.SuspendLayout();

            // dataGridView
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(760, 300);
            this.dataGridView.TabIndex = 0;

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(12, 320);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 30);
            this.btnAdd.Text = "Добавить книгу";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(138, 320);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(244, 320);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // btnFindMin
            this.btnFindMin.Location = new System.Drawing.Point(350, 320);
            this.btnFindMin.Name = "btnFindMin";
            this.btnFindMin.Size = new System.Drawing.Size(120, 30);
            this.btnFindMin.Text = "Найти мин. штраф";
            this.btnFindMin.UseVisualStyleBackColor = true;
            this.btnFindMin.Click += new System.EventHandler(this.btnFindMin_Click);

            // btnExport
            this.btnExport.Location = new System.Drawing.Point(12, 360);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 30);
            this.btnExport.Text = "Экспорт";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // btnImport
            this.btnImport.Location = new System.Drawing.Point(118, 360);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 30);
            this.btnImport.Text = "Импорт";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(224, 360);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 30);
            this.btnRefresh.Text = "Обновить из БД";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // btnClearDb
            this.btnClearDb.Location = new System.Drawing.Point(350, 360);
            this.btnClearDb.Name = "btnClearDb";
            this.btnClearDb.Size = new System.Drawing.Size(100, 30);
            this.btnClearDb.Text = "Очистить БД";
            this.btnClearDb.UseVisualStyleBackColor = true;
            this.btnClearDb.Click += new System.EventHandler(this.btnClearDb_Click);

            // statusStrip
            this.statusStrip.Items.AddRange(new ToolStripItem[] {
                this.lblStatus
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 400);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);

            // lblStatus
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(769, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 422);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnClearDb);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnFindMin);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView);
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            this.Name = "MainForm";
            this.Text = "Библиотечная система v1.0";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}