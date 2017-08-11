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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.grb_Фильтр = new System.Windows.Forms.GroupBox();
            this.grb_Main = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.grb_ManageData = new System.Windows.Forms.GroupBox();
            this.groupBoxSourseShedule = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.rbSourseSheduleCis = new System.Windows.Forms.RadioButton();
            this.rbSourseSheduleLocal = new System.Windows.Forms.RadioButton();
            this.btn_Сохранить = new System.Windows.Forms.Button();
            this.btn_УдалитьЗапись = new System.Windows.Forms.Button();
            this.btn_ДобавитьЗапись = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).BeginInit();
            this.exp_Filter.Content.SuspendLayout();
            this.grb_Фильтр.SuspendLayout();
            this.grb_Main.SuspendLayout();
            this.grb_ManageData.SuspendLayout();
            this.groupBoxSourseShedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_TrainTable
            // 
            this.dgv_TrainTable.AllowUserToAddRows = false;
            this.dgv_TrainTable.AllowUserToDeleteRows = false;
            this.dgv_TrainTable.AllowUserToOrderColumns = true;
            this.dgv_TrainTable.AllowUserToResizeRows = false;
            this.dgv_TrainTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_TrainTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_TrainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightGreen;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_TrainTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_TrainTable.Location = new System.Drawing.Point(0, 171);
            this.dgv_TrainTable.MultiSelect = false;
            this.dgv_TrainTable.Name = "dgv_TrainTable";
            this.dgv_TrainTable.ReadOnly = true;
            this.dgv_TrainTable.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.dgv_TrainTable.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_TrainTable.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_TrainTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dgv_TrainTable.RowTemplate.Height = 30;
            this.dgv_TrainTable.RowTemplate.ReadOnly = true;
            this.dgv_TrainTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_TrainTable.ShowEditingIcon = false;
            this.dgv_TrainTable.Size = new System.Drawing.Size(1109, 335);
            this.dgv_TrainTable.TabIndex = 0;
            this.dgv_TrainTable.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_TrainTable_ColumnDisplayIndexChanged);
            this.dgv_TrainTable.DoubleClick += new System.EventHandler(this.dgv_TrainTable_DoubleClick);
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
            this.exp_Filter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.exp_Filter.CollapseImage = ((System.Drawing.Image)(resources.GetObject("exp_Filter.CollapseImage")));
            // 
            // exp_Filter.Content
            // 
            this.exp_Filter.Content.AutoScroll = true;
            this.exp_Filter.Content.AutoScrollMinSize = new System.Drawing.Size(150, 50);
            this.exp_Filter.Content.Controls.Add(this.grb_Фильтр);
            this.exp_Filter.Content.Controls.Add(this.grb_Main);
            this.exp_Filter.ExpandImage = ((System.Drawing.Image)(resources.GetObject("exp_Filter.ExpandImage")));
            this.exp_Filter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exp_Filter.Header = "Панель";
            this.exp_Filter.Location = new System.Drawing.Point(0, 0);
            this.exp_Filter.MinimumSize = new System.Drawing.Size(0, 53);
            this.exp_Filter.Name = "exp_Filter";
            this.exp_Filter.Size = new System.Drawing.Size(1109, 170);
            this.exp_Filter.TabIndex = 14;
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
            // grb_Main
            // 
            this.grb_Main.Controls.Add(this.label11);
            this.grb_Main.Controls.Add(this.checkBox1);
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
            this.grb_Main.Size = new System.Drawing.Size(596, 128);
            this.grb_Main.TabIndex = 15;
            this.grb_Main.TabStop = false;
            this.grb_Main.Text = "Форматирование";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(229, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 16);
            this.label11.TabIndex = 19;
            this.label11.Text = "Стоянка";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(253, 48);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Tag = "ВремяОтправления";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btn_SaveTableFormat
            // 
            this.btn_SaveTableFormat.Location = new System.Drawing.Point(476, 89);
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
            this.label6.Location = new System.Drawing.Point(489, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Дни след.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(407, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Маршрут";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(302, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Время отпр.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(125, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 16);
            this.label8.TabIndex = 14;
            this.label8.Text = "Время приб.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(65, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 16);
            this.label9.TabIndex = 13;
            this.label9.Text = "Номер";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 16);
            this.label10.TabIndex = 12;
            this.label10.Text = "Id";
            // 
            // chb_ДниСледования
            // 
            this.chb_ДниСледования.AutoSize = true;
            this.chb_ДниСледования.Location = new System.Drawing.Point(517, 48);
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
            this.chb_Маршрут.Location = new System.Drawing.Point(433, 48);
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
            this.chb_ВремяОтпр.Location = new System.Drawing.Point(338, 48);
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
            // grb_ManageData
            // 
            this.grb_ManageData.Controls.Add(this.groupBoxSourseShedule);
            this.grb_ManageData.Controls.Add(this.btn_Сохранить);
            this.grb_ManageData.Controls.Add(this.btn_УдалитьЗапись);
            this.grb_ManageData.Controls.Add(this.btn_ДобавитьЗапись);
            this.grb_ManageData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grb_ManageData.Location = new System.Drawing.Point(0, 538);
            this.grb_ManageData.Name = "grb_ManageData";
            this.grb_ManageData.Size = new System.Drawing.Size(1109, 55);
            this.grb_ManageData.TabIndex = 15;
            this.grb_ManageData.TabStop = false;
            // 
            // groupBoxSourseShedule
            // 
            this.groupBoxSourseShedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSourseShedule.Controls.Add(this.btnLoad);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleCis);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleLocal);
            this.groupBoxSourseShedule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSourseShedule.Location = new System.Drawing.Point(5, -5);
            this.groupBoxSourseShedule.Name = "groupBoxSourseShedule";
            this.groupBoxSourseShedule.Size = new System.Drawing.Size(442, 54);
            this.groupBoxSourseShedule.TabIndex = 26;
            this.groupBoxSourseShedule.TabStop = false;
            this.groupBoxSourseShedule.Text = "Источник загрузки расписания";
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(239, 18);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(187, 29);
            this.btnLoad.TabIndex = 30;
            this.btnLoad.Text = "Загрузить расписание";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // rbSourseSheduleCis
            // 
            this.rbSourseSheduleCis.AutoSize = true;
            this.rbSourseSheduleCis.Location = new System.Drawing.Point(132, 23);
            this.rbSourseSheduleCis.Name = "rbSourseSheduleCis";
            this.rbSourseSheduleCis.Size = new System.Drawing.Size(62, 25);
            this.rbSourseSheduleCis.TabIndex = 2;
            this.rbSourseSheduleCis.TabStop = true;
            this.rbSourseSheduleCis.Text = "ЦИС";
            this.rbSourseSheduleCis.UseVisualStyleBackColor = true;
            // 
            // rbSourseSheduleLocal
            // 
            this.rbSourseSheduleLocal.AutoSize = true;
            this.rbSourseSheduleLocal.Checked = true;
            this.rbSourseSheduleLocal.Location = new System.Drawing.Point(15, 23);
            this.rbSourseSheduleLocal.Name = "rbSourseSheduleLocal";
            this.rbSourseSheduleLocal.Size = new System.Drawing.Size(106, 25);
            this.rbSourseSheduleLocal.TabIndex = 1;
            this.rbSourseSheduleLocal.TabStop = true;
            this.rbSourseSheduleLocal.Text = "локальный";
            this.rbSourseSheduleLocal.UseVisualStyleBackColor = true;
            // 
            // btn_Сохранить
            // 
            this.btn_Сохранить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Сохранить.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Сохранить.Location = new System.Drawing.Point(1021, 21);
            this.btn_Сохранить.Name = "btn_Сохранить";
            this.btn_Сохранить.Size = new System.Drawing.Size(77, 29);
            this.btn_Сохранить.TabIndex = 24;
            this.btn_Сохранить.Text = "СОХР.";
            this.btn_Сохранить.UseVisualStyleBackColor = true;
            // 
            // btn_УдалитьЗапись
            // 
            this.btn_УдалитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_УдалитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_УдалитьЗапись.Location = new System.Drawing.Point(864, 21);
            this.btn_УдалитьЗапись.Name = "btn_УдалитьЗапись";
            this.btn_УдалитьЗапись.Size = new System.Drawing.Size(151, 29);
            this.btn_УдалитьЗапись.TabIndex = 23;
            this.btn_УдалитьЗапись.Text = "Удалить запись";
            this.btn_УдалитьЗапись.UseVisualStyleBackColor = true;
            // 
            // btn_ДобавитьЗапись
            // 
            this.btn_ДобавитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ДобавитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ДобавитьЗапись.Location = new System.Drawing.Point(707, 20);
            this.btn_ДобавитьЗапись.Name = "btn_ДобавитьЗапись";
            this.btn_ДобавитьЗапись.Size = new System.Drawing.Size(151, 29);
            this.btn_ДобавитьЗапись.TabIndex = 22;
            this.btn_ДобавитьЗапись.Text = "Добавить запись";
            this.btn_ДобавитьЗапись.UseVisualStyleBackColor = true;
            // 
            // TrainTableGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 593);
            this.Controls.Add(this.grb_ManageData);
            this.Controls.Add(this.exp_Filter);
            this.Controls.Add(this.dgv_TrainTable);
            this.Name = "TrainTableGrid";
            this.Text = "Расписание движения поездов";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TrainTable)).EndInit();
            this.exp_Filter.Content.ResumeLayout(false);
            this.grb_Фильтр.ResumeLayout(false);
            this.grb_Фильтр.PerformLayout();
            this.grb_Main.ResumeLayout(false);
            this.grb_Main.PerformLayout();
            this.grb_ManageData.ResumeLayout(false);
            this.groupBoxSourseShedule.ResumeLayout(false);
            this.groupBoxSourseShedule.PerformLayout();
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox grb_ManageData;
        private System.Windows.Forms.Button btn_Сохранить;
        private System.Windows.Forms.Button btn_УдалитьЗапись;
        private System.Windows.Forms.Button btn_ДобавитьЗапись;
        private System.Windows.Forms.GroupBox groupBoxSourseShedule;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.RadioButton rbSourseSheduleCis;
        private System.Windows.Forms.RadioButton rbSourseSheduleLocal;
    }
}

