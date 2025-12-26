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
        private Button btnView;
        private Button btnFindMin;
        private Button btnExport;
        private Button btnImport;
        private Button btnRefresh;
        private Button btnClearDb;
        private Button btnSortReset;
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnFindMin = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClearDb = new System.Windows.Forms.Button();
            this.btnSortReset = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(9, 10);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(713, 244);
            this.dataGridView.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(9, 260);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 24);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(88, 260);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(105, 24);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(197, 260);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 24);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(261, 260);
            this.btnView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(68, 24);
            this.btnView.TabIndex = 7;
            this.btnView.Text = "Просмотр";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnFindMin
            // 
            this.btnFindMin.Location = new System.Drawing.Point(333, 260);
            this.btnFindMin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFindMin.Name = "btnFindMin";
            this.btnFindMin.Size = new System.Drawing.Size(90, 24);
            this.btnFindMin.TabIndex = 6;
            this.btnFindMin.Text = "Найти мин. штраф";
            this.btnFindMin.UseVisualStyleBackColor = true;
            this.btnFindMin.Click += new System.EventHandler(this.btnFindMin_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(9, 292);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 24);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Экспорт";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(74, 292);
            this.btnImport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(60, 24);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Импорт";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(138, 292);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(112, 24);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Обновить из БД";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClearDb
            // 
            this.btnClearDb.Location = new System.Drawing.Point(254, 292);
            this.btnClearDb.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClearDb.Name = "btnClearDb";
            this.btnClearDb.Size = new System.Drawing.Size(75, 24);
            this.btnClearDb.TabIndex = 2;
            this.btnClearDb.Text = "Очистить БД";
            this.btnClearDb.UseVisualStyleBackColor = true;
            this.btnClearDb.Click += new System.EventHandler(this.btnClearDb_Click);
            // 
            // btnSortReset
            // 
            this.btnSortReset.Location = new System.Drawing.Point(333, 292);
            this.btnSortReset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSortReset.Name = "btnSortReset";
            this.btnSortReset.Size = new System.Drawing.Size(90, 24);
            this.btnSortReset.TabIndex = 1;
            this.btnSortReset.Text = "Сбросить сортировку";
            this.btnSortReset.UseVisualStyleBackColor = true;
            this.btnSortReset.Click += new System.EventHandler(this.btnSortReset_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 321);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip.Size = new System.Drawing.Size(588, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(577, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 343);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnSortReset);
            this.Controls.Add(this.btnClearDb);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnFindMin);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "Библиотечная система v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}