namespace MainExample
{
    partial class AdminForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_пользователи = new System.Windows.Forms.DataGridView();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clPassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clRole = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_пользователи)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_пользователи
            // 
            this.dgv_пользователи.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_пользователи.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_пользователи.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_пользователи.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.date,
            this.clPassword,
            this.clRole});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_пользователи.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_пользователи.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_пользователи.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgv_пользователи.Location = new System.Drawing.Point(0, 0);
            this.dgv_пользователи.Name = "dgv_пользователи";
            this.dgv_пользователи.Size = new System.Drawing.Size(993, 468);
            this.dgv_пользователи.TabIndex = 2;
            // 
            // date
            // 
            this.date.FillWeight = 350F;
            this.date.HeaderText = "Логин";
            this.date.Name = "date";
            this.date.Width = 350;
            // 
            // clPassword
            // 
            this.clPassword.FillWeight = 300F;
            this.clPassword.HeaderText = "Пароль";
            this.clPassword.Name = "clPassword";
            this.clPassword.Width = 300;
            // 
            // clRole
            // 
            this.clRole.FillWeight = 200F;
            this.clRole.HeaderText = "Роль";
            this.clRole.Name = "clRole";
            this.clRole.Width = 200;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 468);
            this.Controls.Add(this.dgv_пользователи);
            this.Name = "AdminForm";
            this.Text = "Админка";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_пользователи)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_пользователи;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn clPassword;
        private System.Windows.Forms.DataGridViewComboBoxColumn clRole;
    }
}