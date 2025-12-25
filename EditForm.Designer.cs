using System.Windows.Forms;

namespace LibrarySystem
{
    partial class EditForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblPrice;
        private TextBox txtPrice;
        private CheckBox chkExtended;
        private Label lblDays;
        private NumericUpDown numDays;
        private Button btnSave;
        private Button btnCancel;

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
            this.lblTitle = new Label();
            this.txtTitle = new TextBox();
            this.lblPrice = new Label();
            this.txtPrice = new TextBox();
            this.chkExtended = new CheckBox();
            this.lblDays = new Label();
            this.numDays = new NumericUpDown();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            // Настройка контролов
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 16);
            this.lblTitle.Text = "Название книги:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(130, 17);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(250, 22);

            // lblPrice
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(20, 50);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(101, 16);
            this.lblPrice.Text = "Стоимость (руб):";

            // txtPrice
            this.txtPrice.Location = new System.Drawing.Point(130, 47);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 22);

            // chkExtended
            this.chkExtended.AutoSize = true;
            this.chkExtended.Location = new System.Drawing.Point(20, 80);
            this.chkExtended.Name = "chkExtended";
            this.chkExtended.Size = new System.Drawing.Size(133, 20);
            this.chkExtended.Text = "Продление срока";
            this.chkExtended.CheckedChanged += new System.EventHandler(this.chkExtended_CheckedChanged);

            // lblDays
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(20, 110);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(99, 16);
            this.lblDays.Text = "Дней продления:";
            this.lblDays.Enabled = false;

            // numDays
            this.numDays.Enabled = false;
            this.numDays.Location = new System.Drawing.Point(130, 108);
            this.numDays.Minimum = 1;
            this.numDays.Maximum = 30;
            this.numDays.Value = 7;
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(60, 22);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(60, 150);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(170, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // EditForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.numDays);
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.chkExtended);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Книга";

            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}