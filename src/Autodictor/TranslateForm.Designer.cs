namespace MainExample
{
    partial class TranslateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.GVtranslate = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GVtranslate)).BeginInit();
            this.SuspendLayout();
            // 
            // GVtranslate
            // 
            this.GVtranslate.AllowUserToAddRows = false;
            this.GVtranslate.AllowUserToDeleteRows = false;
            this.GVtranslate.AllowUserToResizeColumns = false;
            this.GVtranslate.AllowUserToResizeRows = false;
            this.GVtranslate.ColumnHeadersHeight = 40;
            this.GVtranslate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.GVtranslate.ColumnHeadersVisible = false;
            this.GVtranslate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.GVtranslate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GVtranslate.Location = new System.Drawing.Point(0, 0);
            this.GVtranslate.MultiSelect = false;
            this.GVtranslate.Name = "GVtranslate";
            this.GVtranslate.ReadOnly = true;
            this.GVtranslate.Size = new System.Drawing.Size(443, 287);
            this.GVtranslate.TabIndex = 0;
            // 
            // Column1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "Перевод";
            this.Column1.MinimumWidth = 400;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 400;
            // 
            // TranslateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 287);
            this.Controls.Add(this.GVtranslate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TranslateForm";
            this.Text = "TranslateForm";
            ((System.ComponentModel.ISupportInitialize)(this.GVtranslate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView GVtranslate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}