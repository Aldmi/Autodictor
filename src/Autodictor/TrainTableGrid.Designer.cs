namespace MainExample
{
    partial class TrainTableGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainTableGrid));
            this.dgv_TrainTable = new System.Windows.Forms.DataGridView();
            this.btn_Filter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ВремяПриб = new System.Windows.Forms.TextBox();
            this.tb_ВремяОтпр = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_НомерПоезда = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_ДниСлед = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.exp_Filter = new WinFormsExpander.Expander();
            this.grb_Main = new System.Windows.Forms.GroupBox();
            this.btn_SaveTableFormat = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chb_ДниСледования = new System.Windows.Forms.CheckBox();
            this.chb_Маршрут = new System.Windows.Forms.CheckBox();
            this.chb_ВремяОтпр = new System.Windows.Forms.CheckBox();
            this.chb_ВремяПрибытия = new System.Windows.Forms.CheckBox();
            this.chb_Номер = new System.Windows.Forms.CheckBox();
            this.chb_Id = new System.Windows.Forms.CheckBox();
            this.grb_Фильтр = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).BeginInit();
            this.exp_Filter.Content.SuspendLayout();
            this.grb_Main.SuspendLayout();
            this.grb_Фильтр.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_TrainTable
            // 
            this.dgv_TrainTable.AllowUserToAddRows = false;
            this.dgv_TrainTable.AllowUserToDeleteRows = false;
            this.dgv_TrainTable.AllowUserToOrderColumns = true;
            this.dgv_TrainTable.AllowUserToResizeRows = false;
            this.dgv_TrainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_TrainTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_TrainTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv_TrainTable.Location = new System.Drawing.Point(0, 170);
            this.dgv_TrainTable.MultiSelect = false;
            this.dgv_TrainTable.Name = "dgv_TrainTable";
            this.dgv_TrainTable.ReadOnly = true;
            this.dgv_TrainTable.Size = new System.Drawing.Size(934, 382);
            this.dgv_TrainTable.TabIndex = 0;
            this.dgv_TrainTable.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_TrainTable_ColumnDisplayIndexChanged);
            // 
            // btn_Filter
            // 
            this.btn_Filter.Image = global::MainExample.Properties.Resources.OkImg;
            this.btn_Filter.Location = new System.Drawing.Point(220, 56);
            this.btn_Filter.Name = "btn_Filter";
            this.btn_Filter.Size = new System.Drawing.Size(53, 32);
            this.btn_Filter.TabIndex = 1;
            this.btn_Filter.UseVisualStyleBackColor = true;
            this.btn_Filter.Click += new System.EventHandler(this.btn_Filter_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Врем приб.";
            // 
            // tb_ВремяПриб
            // 
            this.tb_ВремяПриб.Location = new System.Drawing.Point(114, 42);
            this.tb_ВремяПриб.Name = "tb_ВремяПриб";
            this.tb_ВремяПриб.Size = new System.Drawing.Size(100, 22);
            this.tb_ВремяПриб.TabIndex = 4;
            // 
            // tb_ВремяОтпр
            // 
            this.tb_ВремяОтпр.Location = new System.Drawing.Point(114, 70);
            this.tb_ВремяОтпр.Name = "tb_ВремяОтпр";
            this.tb_ВремяОтпр.Size = new System.Drawing.Size(100, 22);
            this.tb_ВремяОтпр.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Время отпр.";
            // 
            // tb_НомерПоезда
            // 
            this.tb_НомерПоезда.Location = new System.Drawing.Point(114, 15);
            this.tb_НомерПоезда.Name = "tb_НомерПоезда";
            this.tb_НомерПоезда.Size = new System.Drawing.Size(100, 22);
            this.tb_НомерПоезда.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Номер поезда";
            // 
            // tb_ДниСлед
            // 
            this.tb_ДниСлед.Location = new System.Drawing.Point(114, 97);
            this.tb_ДниСлед.Name = "tb_ДниСлед";
            this.tb_ДниСлед.Size = new System.Drawing.Size(100, 22);
            this.tb_ДниСлед.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Дни след";
            // 
            // exp_Filter
            // 
            this.exp_Filter.AutoScroll = true;
            this.exp_Filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exp_Filter.CollapseImage = ((System.Drawing.Image)(resources.GetObject("exp_Filter.CollapseImage")));
            // 
            // exp_Filter.Content
            // 
            this.exp_Filter.Content.AutoScroll = true;
            this.exp_Filter.Content.AutoScrollMinSize = new System.Drawing.Size(150, 50);
            this.exp_Filter.Content.Controls.Add(this.grb_Фильтр);
            this.exp_Filter.Content.Controls.Add(this.grb_Main);
            this.exp_Filter.Dock = System.Windows.Forms.DockStyle.Top;
            this.exp_Filter.ExpandImage = ((System.Drawing.Image)(resources.GetObject("exp_Filter.ExpandImage")));
            this.exp_Filter.Header = "Фильтр";
            this.exp_Filter.Location = new System.Drawing.Point(0, 0);
            this.exp_Filter.MinimumSize = new System.Drawing.Size(0, 53);
            this.exp_Filter.Name = "exp_Filter";
            this.exp_Filter.Size = new System.Drawing.Size(934, 170);
            this.exp_Filter.TabIndex = 14;
            // 
            // grb_Main
            // 
            this.grb_Main.Controls.Add(this.btn_SaveTableFormat);
            this.grb_Main.Controls.Add(this.label6);
            this.grb_Main.Controls.Add(this.label5);
            this.grb_Main.Controls.Add(this.label7);
            this.grb_Main.Controls.Add(this.label8);
            this.grb_Main.Controls.Add(this.label9);
            this.grb_Main.Controls.Add(this.label10);
            this.grb_Main.Controls.Add(this.chb_ДниСледования);
            this.grb_Main.Controls.Add(this.chb_Маршрут);
            this.grb_Main.Controls.Add(this.chb_ВремяОтпр);
            this.grb_Main.Controls.Add(this.chb_ВремяПрибытия);
            this.grb_Main.Controls.Add(this.chb_Номер);
            this.grb_Main.Controls.Add(this.chb_Id);
            this.grb_Main.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grb_Main.Location = new System.Drawing.Point(310, 7);
            this.grb_Main.Name = "grb_Main";
            this.grb_Main.Size = new System.Drawing.Size(489, 128);
            this.grb_Main.TabIndex = 15;
            this.grb_Main.TabStop = false;
            this.grb_Main.Text = "Форматирование таблицы";
            // 
            // btn_SaveTableFormat
            // 
            this.btn_SaveTableFormat.Location = new System.Drawing.Point(396, 89);
            this.btn_SaveTableFormat.Name = "btn_SaveTableFormat";
            this.btn_SaveTableFormat.Size = new System.Drawing.Size(85, 29);
            this.btn_SaveTableFormat.TabIndex = 16;
            this.btn_SaveTableFormat.Text = "Сохранить";
            this.btn_SaveTableFormat.UseVisualStyleBackColor = true;
            this.btn_SaveTableFormat.Click += new System.EventHandler(this.btn_SaveTableFormat_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(411, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Дни след.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(330, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Маршрут";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Время отпр.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(129, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 16);
            this.label8.TabIndex = 14;
            this.label8.Text = "Время приб.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 16);
            this.label9.TabIndex = 13;
            this.label9.Text = "Номер";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 16);
            this.label10.TabIndex = 12;
            this.label10.Text = "Id";
            // 
            // chb_ДниСледования
            // 
            this.chb_ДниСледования.AutoSize = true;
            this.chb_ДниСледования.Location = new System.Drawing.Point(437, 48);
            this.chb_ДниСледования.Name = "chb_ДниСледования";
            this.chb_ДниСледования.Size = new System.Drawing.Size(15, 14);
            this.chb_ДниСледования.TabIndex = 5;
            this.chb_ДниСледования.Tag = "ДниСледования";
            this.chb_ДниСледования.UseVisualStyleBackColor = true;
            this.chb_ДниСледования.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_Маршрут
            // 
            this.chb_Маршрут.AutoSize = true;
            this.chb_Маршрут.Location = new System.Drawing.Point(353, 48);
            this.chb_Маршрут.Name = "chb_Маршрут";
            this.chb_Маршрут.Size = new System.Drawing.Size(15, 14);
            this.chb_Маршрут.TabIndex = 4;
            this.chb_Маршрут.Tag = "Маршрут";
            this.chb_Маршрут.UseVisualStyleBackColor = true;
            this.chb_Маршрут.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_ВремяОтпр
            // 
            this.chb_ВремяОтпр.AutoSize = true;
            this.chb_ВремяОтпр.Location = new System.Drawing.Point(258, 48);
            this.chb_ВремяОтпр.Name = "chb_ВремяОтпр";
            this.chb_ВремяОтпр.Size = new System.Drawing.Size(15, 14);
            this.chb_ВремяОтпр.TabIndex = 3;
            this.chb_ВремяОтпр.Tag = "ВремяОтправления";
            this.chb_ВремяОтпр.UseVisualStyleBackColor = true;
            this.chb_ВремяОтпр.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_ВремяПрибытия
            // 
            this.chb_ВремяПрибытия.AutoSize = true;
            this.chb_ВремяПрибытия.Location = new System.Drawing.Point(161, 48);
            this.chb_ВремяПрибытия.Name = "chb_ВремяПрибытия";
            this.chb_ВремяПрибытия.Size = new System.Drawing.Size(15, 14);
            this.chb_ВремяПрибытия.TabIndex = 2;
            this.chb_ВремяПрибытия.Tag = "ВремяПрибытия";
            this.chb_ВремяПрибытия.UseVisualStyleBackColor = true;
            this.chb_ВремяПрибытия.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_Номер
            // 
            this.chb_Номер.AutoSize = true;
            this.chb_Номер.Location = new System.Drawing.Point(83, 48);
            this.chb_Номер.Name = "chb_Номер";
            this.chb_Номер.Size = new System.Drawing.Size(15, 14);
            this.chb_Номер.TabIndex = 1;
            this.chb_Номер.Tag = "Номер";
            this.chb_Номер.UseVisualStyleBackColor = true;
            this.chb_Номер.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // chb_Id
            // 
            this.chb_Id.AutoSize = true;
            this.chb_Id.Location = new System.Drawing.Point(18, 48);
            this.chb_Id.Name = "chb_Id";
            this.chb_Id.Size = new System.Drawing.Size(15, 14);
            this.chb_Id.TabIndex = 0;
            this.chb_Id.Tag = "Id";
            this.chb_Id.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chb_Id.UseVisualStyleBackColor = true;
            this.chb_Id.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // grb_Фильтр
            // 
            this.grb_Фильтр.Controls.Add(this.btn_Filter);
            this.grb_Фильтр.Controls.Add(this.label3);
            this.grb_Фильтр.Controls.Add(this.tb_ДниСлед);
            this.grb_Фильтр.Controls.Add(this.label1);
            this.grb_Фильтр.Controls.Add(this.label4);
            this.grb_Фильтр.Controls.Add(this.label2);
            this.grb_Фильтр.Controls.Add(this.tb_ВремяПриб);
            this.grb_Фильтр.Controls.Add(this.tb_ВремяОтпр);
            this.grb_Фильтр.Controls.Add(this.tb_НомерПоезда);
            this.grb_Фильтр.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grb_Фильтр.Location = new System.Drawing.Point(3, 6);
            this.grb_Фильтр.Name = "grb_Фильтр";
            this.grb_Фильтр.Size = new System.Drawing.Size(282, 129);
            this.grb_Фильтр.TabIndex = 16;
            this.grb_Фильтр.TabStop = false;
            this.grb_Фильтр.Text = "Фильтр";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 552);
            this.Controls.Add(this.exp_Filter);
            this.Controls.Add(this.dgv_TrainTable);
            this.Name = "TrainTableGrid";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).EndInit();
            this.exp_Filter.Content.ResumeLayout(false);
            this.grb_Main.ResumeLayout(false);
            this.grb_Main.PerformLayout();
            this.grb_Фильтр.ResumeLayout(false);
            this.grb_Фильтр.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_TrainTable;
        private System.Windows.Forms.Button btn_Filter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ВремяПриб;
        private System.Windows.Forms.TextBox tb_ВремяОтпр;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_НомерПоезда;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_ДниСлед;
        private System.Windows.Forms.Label label4;
        private WinFormsExpander.Expander exp_Filter;
        private System.Windows.Forms.GroupBox grb_Main;
        private System.Windows.Forms.Button btn_SaveTableFormat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chb_ДниСледования;
        private System.Windows.Forms.CheckBox chb_Маршрут;
        private System.Windows.Forms.CheckBox chb_ВремяОтпр;
        private System.Windows.Forms.CheckBox chb_ВремяПрибытия;
        private System.Windows.Forms.CheckBox chb_Номер;
        private System.Windows.Forms.CheckBox chb_Id;
        private System.Windows.Forms.GroupBox grb_Фильтр;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

