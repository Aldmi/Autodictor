namespace MainExample
{
    partial class СписокВоспроизведения
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
            this.lVСписокФайлов = new System.Windows.Forms.ListView();
            this.btnОбновить = new System.Windows.Forms.Button();
            this.btnОчистить = new System.Windows.Forms.Button();
            this.btnУдалитьВыделенные = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lVСписокФайлов
            // 
            this.lVСписокФайлов.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lVСписокФайлов.FullRowSelect = true;
            this.lVСписокФайлов.Location = new System.Drawing.Point(0, 0);
            this.lVСписокФайлов.Name = "lVСписокФайлов";
            this.lVСписокФайлов.Size = new System.Drawing.Size(402, 636);
            this.lVСписокФайлов.TabIndex = 0;
            this.lVСписокФайлов.UseCompatibleStateImageBehavior = false;
            this.lVСписокФайлов.View = System.Windows.Forms.View.Details;
            // 
            // btnОбновить
            // 
            this.btnОбновить.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnОбновить.Location = new System.Drawing.Point(409, 13);
            this.btnОбновить.Name = "btnОбновить";
            this.btnОбновить.Size = new System.Drawing.Size(162, 40);
            this.btnОбновить.TabIndex = 1;
            this.btnОбновить.Text = "ОБНОВИТЬ";
            this.btnОбновить.UseVisualStyleBackColor = true;
            this.btnОбновить.Click += new System.EventHandler(this.btnОбновить_Click);
            // 
            // btnОчистить
            // 
            this.btnОчистить.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnОчистить.Location = new System.Drawing.Point(408, 59);
            this.btnОчистить.Name = "btnОчистить";
            this.btnОчистить.Size = new System.Drawing.Size(163, 40);
            this.btnОчистить.TabIndex = 2;
            this.btnОчистить.Text = "ОЧИСТИТЬ";
            this.btnОчистить.UseVisualStyleBackColor = true;
            this.btnОчистить.Click += new System.EventHandler(this.btnОчистить_Click);
            // 
            // btnУдалитьВыделенные
            // 
            this.btnУдалитьВыделенные.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnУдалитьВыделенные.Location = new System.Drawing.Point(409, 139);
            this.btnУдалитьВыделенные.Name = "btnУдалитьВыделенные";
            this.btnУдалитьВыделенные.Size = new System.Drawing.Size(162, 59);
            this.btnУдалитьВыделенные.TabIndex = 3;
            this.btnУдалитьВыделенные.Text = "УДАЛИТЬ ВЫДЕЛЕННЫЕ";
            this.btnУдалитьВыделенные.UseVisualStyleBackColor = true;
            this.btnУдалитьВыделенные.Click += new System.EventHandler(this.btnУдалитьВыделенные_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Имена файлов";
            this.columnHeader1.Width = 500;
            // 
            // СписокВоспроизведения
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 638);
            this.Controls.Add(this.btnУдалитьВыделенные);
            this.Controls.Add(this.btnОчистить);
            this.Controls.Add(this.btnОбновить);
            this.Controls.Add(this.lVСписокФайлов);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "СписокВоспроизведения";
            this.Text = "Список воспроизведения";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lVСписокФайлов;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnОбновить;
        private System.Windows.Forms.Button btnОчистить;
        private System.Windows.Forms.Button btnУдалитьВыделенные;
    }
}