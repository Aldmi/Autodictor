namespace TestGrid4WinForms
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.btn_DisableColumn = new System.Windows.Forms.Button();
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).BeginInit();
            this.exp_Filter.Content.SuspendLayout();
            this.grb_Main.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_TrainTable
            // 
            this.dgv_TrainTable.AllowUserToAddRows = false;
            this.dgv_TrainTable.AllowUserToDeleteRows = false;
            this.dgv_TrainTable.AllowUserToOrderColumns = true;
            this.dgv_TrainTable.AllowUserToResizeRows = false;
            this.dgv_TrainTable.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgv_TrainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_TrainTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_TrainTable.Location = new System.Drawing.Point(0, 243);
            this.dgv_TrainTable.MultiSelect = false;
            this.dgv_TrainTable.Name = "dgv_TrainTable";
            this.dgv_TrainTable.ReadOnly = true;
            this.dgv_TrainTable.Size = new System.Drawing.Size(934, 183);
            this.dgv_TrainTable.TabIndex = 0;
            this.dgv_TrainTable.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_TrainTable_ColumnDisplayIndexChanged);
            this.dgv_TrainTable.Sorted += new System.EventHandler(this.dgv_TrainTable_Sorted);
            // 
            // btn_Filter
            // 
            this.btn_Filter.Location = new System.Drawing.Point(147, 149);
            this.btn_Filter.Name = "btn_Filter";
            this.btn_Filter.Size = new System.Drawing.Size(100, 47);
            this.btn_Filter.TabIndex = 1;
            this.btn_Filter.Text = "Filter";
            this.btn_Filter.UseVisualStyleBackColor = true;
            this.btn_Filter.Click += new System.EventHandler(this.btn_Filter_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Врем прибытия";
            // 
            // tb_ВремяПриб
            // 
            this.tb_ВремяПриб.Location = new System.Drawing.Point(25, 81);
            this.tb_ВремяПриб.Name = "tb_ВремяПриб";
            this.tb_ВремяПриб.Size = new System.Drawing.Size(100, 20);
            this.tb_ВремяПриб.TabIndex = 4;
            // 
            // tb_ВремяОтпр
            // 
            this.tb_ВремяОтпр.Location = new System.Drawing.Point(25, 127);
            this.tb_ВремяОтпр.Name = "tb_ВремяОтпр";
            this.tb_ВремяОтпр.Size = new System.Drawing.Size(100, 20);
            this.tb_ВремяОтпр.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Время отправления";
            // 
            // tb_НомерПоезда
            // 
            this.tb_НомерПоезда.Location = new System.Drawing.Point(25, 34);
            this.tb_НомерПоезда.Name = "tb_НомерПоезда";
            this.tb_НомерПоезда.Size = new System.Drawing.Size(100, 20);
            this.tb_НомерПоезда.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Номер поезда";
            // 
            // tb_ДниСлед
            // 
            this.tb_ДниСлед.Location = new System.Drawing.Point(25, 176);
            this.tb_ДниСлед.Name = "tb_ДниСлед";
            this.tb_ДниСлед.Size = new System.Drawing.Size(100, 20);
            this.tb_ДниСлед.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Дни след";
            // 
            // btn_DisableColumn
            // 
            this.btn_DisableColumn.Location = new System.Drawing.Point(0, 0);
            this.btn_DisableColumn.Name = "btn_DisableColumn";
            this.btn_DisableColumn.Size = new System.Drawing.Size(75, 23);
            this.btn_DisableColumn.TabIndex = 15;
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
            this.exp_Filter.Content.Controls.Add(this.grb_Main);
            this.exp_Filter.Content.Controls.Add(this.label3);
            this.exp_Filter.Content.Controls.Add(this.label1);
            this.exp_Filter.Content.Controls.Add(this.tb_ВремяПриб);
            this.exp_Filter.Content.Controls.Add(this.btn_Filter);
            this.exp_Filter.Content.Controls.Add(this.tb_ДниСлед);
            this.exp_Filter.Content.Controls.Add(this.label2);
            this.exp_Filter.Content.Controls.Add(this.label4);
            this.exp_Filter.Content.Controls.Add(this.tb_ВремяОтпр);
            this.exp_Filter.Content.Controls.Add(this.tb_НомерПоезда);
            this.exp_Filter.Dock = System.Windows.Forms.DockStyle.Top;
            this.exp_Filter.ExpandImage = ((System.Drawing.Image)(resources.GetObject("exp_Filter.ExpandImage")));
            this.exp_Filter.Header = "Фильтр";
            this.exp_Filter.Location = new System.Drawing.Point(0, 0);
            this.exp_Filter.MinimumSize = new System.Drawing.Size(0, 53);
            this.exp_Filter.Name = "exp_Filter";
            this.exp_Filter.Size = new System.Drawing.Size(934, 239);
            this.exp_Filter.TabIndex = 14;
            this.exp_Filter.ExpandedChanged += new System.EventHandler(this.exp_Filter_ExpandedChanged_1);
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
            this.grb_Main.Location = new System.Drawing.Point(438, 18);
            this.grb_Main.Name = "grb_Main";
            this.grb_Main.Size = new System.Drawing.Size(489, 128);
            this.grb_Main.TabIndex = 15;
            this.grb_Main.TabStop = false;
            this.grb_Main.Text = "Форматирование таблицы";
            // 
            // btn_SaveTableFormat
            // 
            this.btn_SaveTableFormat.Location = new System.Drawing.Point(381, 93);
            this.btn_SaveTableFormat.Name = "btn_SaveTableFormat";
            this.btn_SaveTableFormat.Size = new System.Drawing.Size(100, 29);
            this.btn_SaveTableFormat.TabIndex = 16;
            this.btn_SaveTableFormat.Text = "Сохранить";
            this.btn_SaveTableFormat.UseVisualStyleBackColor = true;
            this.btn_SaveTableFormat.Click += new System.EventHandler(this.btn_SaveTableFormat_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(411, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Дни след.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(330, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Маршрут";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Время отпр.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(129, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 16);
            this.label8.TabIndex = 14;
            this.label8.Text = "Время приб.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 16);
            this.label9.TabIndex = 13;
            this.label9.Text = "Номер";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 16);
            this.label10.TabIndex = 12;
            this.label10.Text = "Id";
            // 
            // chb_ДниСледования
            // 
            this.chb_ДниСледования.AutoSize = true;
            this.chb_ДниСледования.Location = new System.Drawing.Point(437, 73);
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
            this.chb_Маршрут.Location = new System.Drawing.Point(353, 73);
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
            this.chb_ВремяОтпр.Location = new System.Drawing.Point(258, 73);
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
            this.chb_ВремяПрибытия.Location = new System.Drawing.Point(161, 73);
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
            this.chb_Номер.Location = new System.Drawing.Point(83, 73);
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
            this.chb_Id.Location = new System.Drawing.Point(18, 73);
            this.chb_Id.Name = "chb_Id";
            this.chb_Id.Size = new System.Drawing.Size(15, 14);
            this.chb_Id.TabIndex = 0;
            this.chb_Id.Tag = "Id";
            this.chb_Id.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chb_Id.UseVisualStyleBackColor = true;
            this.chb_Id.CheckedChanged += new System.EventHandler(this.chb_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 432);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(928, 100);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(920, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(920, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(920, 74);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 552);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.exp_Filter);
            this.Controls.Add(this.btn_DisableColumn);
            this.Controls.Add(this.dgv_TrainTable);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).EndInit();
            this.exp_Filter.Content.ResumeLayout(false);
            this.exp_Filter.Content.PerformLayout();
            this.grb_Main.ResumeLayout(false);
            this.grb_Main.PerformLayout();
            this.tabControl1.ResumeLayout(false);
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
        private System.Windows.Forms.Button btn_DisableColumn;
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
    }
}

