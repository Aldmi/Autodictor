namespace MainExample
{
    partial class TrainTable
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tB_Номер = new System.Windows.Forms.TextBox();
            this.tB_Название = new System.Windows.Forms.TextBox();
            this.Название = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cB_Прибытие = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cB_Отправление = new System.Windows.Forms.CheckBox();
            this.dTP_Прибытие = new System.Windows.Forms.DateTimePicker();
            this.dTP_Отправление = new System.Windows.Forms.DateTimePicker();
            this.tBПримечание = new System.Windows.Forms.TextBox();
            this.btn_ДобавитьЗапись = new System.Windows.Forms.Button();
            this.btn_ИзменитьЗапись = new System.Windows.Forms.Button();
            this.btn_УдалитьЗапись = new System.Windows.Forms.Button();
            this.btn_Вверх = new System.Windows.Forms.Button();
            this.btn_Вниз = new System.Windows.Forms.Button();
            this.btn_Сохранить = new System.Windows.Forms.Button();
            this.btn_ШаблонОповещения = new System.Windows.Forms.Button();
            this.gB_НумерацияПоезда = new System.Windows.Forms.GroupBox();
            this.rB_Нумерация_СХвоста = new System.Windows.Forms.RadioButton();
            this.rB_Нумерация_СГоловы = new System.Windows.Forms.RadioButton();
            this.rB_Нумерация_Отсутствует = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cB_НомерПути = new System.Windows.Forms.ComboBox();
            this.gBОтображениеРасписания = new System.Windows.Forms.GroupBox();
            this.rBОтображениеПригород = new System.Windows.Forms.RadioButton();
            this.rBОтображениеДальнегоСледования = new System.Windows.Forms.RadioButton();
            this.rBОтображениеОтсутствует = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.lblСостояниеCIS = new System.Windows.Forms.Label();
            this.pnСостояниеCIS = new System.Windows.Forms.Panel();
            this.groupBoxSourseShedule = new System.Windows.Forms.GroupBox();
            this.rbSourseSheduleCis = new System.Windows.Forms.RadioButton();
            this.rbSourseSheduleLocal = new System.Windows.Forms.RadioButton();
            this.btnLoad = new System.Windows.Forms.Button();
            this.gB_НумерацияПоезда.SuspendLayout();
            this.gBОтображениеРасписания.SuspendLayout();
            this.pnСостояниеCIS.SuspendLayout();
            this.groupBoxSourseShedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 1);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1471, 461);
            this.listView1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.listView1, "Расписание движения поездов");
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Номер";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Название";
            this.columnHeader3.Width = 600;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Прибытие";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Стоянка";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Отправление";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Дни следования";
            this.columnHeader7.Width = 700;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(691, 581);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(463, 37);
            this.button1.TabIndex = 1;
            this.button1.Text = "Редактировать расписание";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(16, 537);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Номер";
            // 
            // tB_Номер
            // 
            this.tB_Номер.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tB_Номер.Location = new System.Drawing.Point(95, 540);
            this.tB_Номер.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tB_Номер.Name = "tB_Номер";
            this.tB_Номер.Size = new System.Drawing.Size(132, 22);
            this.tB_Номер.TabIndex = 3;
            this.toolTip1.SetToolTip(this.tB_Номер, "Поле ввода номера поезда");
            // 
            // tB_Название
            // 
            this.tB_Название.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tB_Название.Location = new System.Drawing.Point(343, 540);
            this.tB_Название.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tB_Название.Name = "tB_Название";
            this.tB_Название.Size = new System.Drawing.Size(421, 22);
            this.tB_Название.TabIndex = 5;
            this.toolTip1.SetToolTip(this.tB_Название, "Поле ввода названия поезда");
            // 
            // Название
            // 
            this.Название.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Название.AutoSize = true;
            this.Название.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Название.Location = new System.Drawing.Point(236, 539);
            this.Название.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Название.Name = "Название";
            this.Название.Size = new System.Drawing.Size(100, 28);
            this.Название.TabIndex = 4;
            this.Название.Text = "Название";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(16, 566);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 28);
            this.label2.TabIndex = 6;
            this.label2.Text = "Прибытие";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(335, 565);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 28);
            this.label4.TabIndex = 10;
            this.label4.Text = "Отправление";
            // 
            // cB_Прибытие
            // 
            this.cB_Прибытие.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cB_Прибытие.AutoSize = true;
            this.cB_Прибытие.Checked = true;
            this.cB_Прибытие.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cB_Прибытие.Location = new System.Drawing.Point(276, 572);
            this.cB_Прибытие.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cB_Прибытие.Name = "cB_Прибытие";
            this.cB_Прибытие.Size = new System.Drawing.Size(34, 21);
            this.cB_Прибытие.TabIndex = 12;
            this.cB_Прибытие.Text = " ";
            this.toolTip1.SetToolTip(this.cB_Прибытие, "Разрешает использование времени прибытия поезда");
            this.cB_Прибытие.UseVisualStyleBackColor = true;
            // 
            // cB_Отправление
            // 
            this.cB_Отправление.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cB_Отправление.AutoSize = true;
            this.cB_Отправление.Checked = true;
            this.cB_Отправление.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cB_Отправление.Location = new System.Drawing.Point(627, 574);
            this.cB_Отправление.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cB_Отправление.Name = "cB_Отправление";
            this.cB_Отправление.Size = new System.Drawing.Size(34, 21);
            this.cB_Отправление.TabIndex = 14;
            this.cB_Отправление.Text = " ";
            this.toolTip1.SetToolTip(this.cB_Отправление, "Разрешает использования времени отправления поезда");
            this.cB_Отправление.UseVisualStyleBackColor = true;
            // 
            // dTP_Прибытие
            // 
            this.dTP_Прибытие.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dTP_Прибытие.CustomFormat = "HH:mm:ss";
            this.dTP_Прибытие.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Прибытие.Location = new System.Drawing.Point(133, 569);
            this.dTP_Прибытие.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dTP_Прибытие.Name = "dTP_Прибытие";
            this.dTP_Прибытие.ShowUpDown = true;
            this.dTP_Прибытие.Size = new System.Drawing.Size(132, 22);
            this.dTP_Прибытие.TabIndex = 18;
            this.toolTip1.SetToolTip(this.dTP_Прибытие, "Поле установки времени прибытия поезда");
            // 
            // dTP_Отправление
            // 
            this.dTP_Отправление.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dTP_Отправление.CustomFormat = "HH:mm:ss";
            this.dTP_Отправление.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Отправление.Location = new System.Drawing.Point(485, 569);
            this.dTP_Отправление.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dTP_Отправление.Name = "dTP_Отправление";
            this.dTP_Отправление.ShowUpDown = true;
            this.dTP_Отправление.Size = new System.Drawing.Size(132, 22);
            this.dTP_Отправление.TabIndex = 19;
            this.toolTip1.SetToolTip(this.dTP_Отправление, "Поле установки времени прибытия поезда");
            // 
            // tBПримечание
            // 
            this.tBПримечание.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tBПримечание.Location = new System.Drawing.Point(764, 688);
            this.tBПримечание.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tBПримечание.Name = "tBПримечание";
            this.tBПримечание.Size = new System.Drawing.Size(389, 22);
            this.tBПримечание.TabIndex = 27;
            this.toolTip1.SetToolTip(this.tBПримечание, "Поле ввода номера поезда");
            // 
            // btn_ДобавитьЗапись
            // 
            this.btn_ДобавитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ДобавитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ДобавитьЗапись.Location = new System.Drawing.Point(1163, 542);
            this.btn_ДобавитьЗапись.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ДобавитьЗапись.Name = "btn_ДобавитьЗапись";
            this.btn_ДобавитьЗапись.Size = new System.Drawing.Size(201, 36);
            this.btn_ДобавитьЗапись.TabIndex = 15;
            this.btn_ДобавитьЗапись.Text = "Добавить запись";
            this.btn_ДобавитьЗапись.UseVisualStyleBackColor = true;
            this.btn_ДобавитьЗапись.Click += new System.EventHandler(this.btn_ДобавитьЗапись_Click);
            // 
            // btn_ИзменитьЗапись
            // 
            this.btn_ИзменитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ИзменитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ИзменитьЗапись.Location = new System.Drawing.Point(1163, 581);
            this.btn_ИзменитьЗапись.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ИзменитьЗапись.Name = "btn_ИзменитьЗапись";
            this.btn_ИзменитьЗапись.Size = new System.Drawing.Size(201, 36);
            this.btn_ИзменитьЗапись.TabIndex = 16;
            this.btn_ИзменитьЗапись.Text = "Изменить запись";
            this.btn_ИзменитьЗапись.UseVisualStyleBackColor = true;
            this.btn_ИзменитьЗапись.Click += new System.EventHandler(this.btn_ИзменитьЗапись_Click);
            // 
            // btn_УдалитьЗапись
            // 
            this.btn_УдалитьЗапись.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_УдалитьЗапись.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_УдалитьЗапись.Location = new System.Drawing.Point(1163, 624);
            this.btn_УдалитьЗапись.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_УдалитьЗапись.Name = "btn_УдалитьЗапись";
            this.btn_УдалитьЗапись.Size = new System.Drawing.Size(201, 36);
            this.btn_УдалитьЗапись.TabIndex = 17;
            this.btn_УдалитьЗапись.Text = "Удалить запись";
            this.btn_УдалитьЗапись.UseVisualStyleBackColor = true;
            this.btn_УдалитьЗапись.Click += new System.EventHandler(this.btn_УдалитьЗапись_Click);
            // 
            // btn_Вверх
            // 
            this.btn_Вверх.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Вверх.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Вверх.Location = new System.Drawing.Point(1372, 542);
            this.btn_Вверх.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Вверх.Name = "btn_Вверх";
            this.btn_Вверх.Size = new System.Drawing.Size(103, 36);
            this.btn_Вверх.TabIndex = 15;
            this.btn_Вверх.Text = "ВВЕРХ";
            this.btn_Вверх.UseVisualStyleBackColor = true;
            this.btn_Вверх.Click += new System.EventHandler(this.btn_Вверх_Click);
            // 
            // btn_Вниз
            // 
            this.btn_Вниз.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Вниз.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Вниз.Location = new System.Drawing.Point(1372, 581);
            this.btn_Вниз.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Вниз.Name = "btn_Вниз";
            this.btn_Вниз.Size = new System.Drawing.Size(103, 36);
            this.btn_Вниз.TabIndex = 20;
            this.btn_Вниз.Text = "ВНИЗ";
            this.btn_Вниз.UseVisualStyleBackColor = true;
            this.btn_Вниз.Click += new System.EventHandler(this.btn_Вниз_Click);
            // 
            // btn_Сохранить
            // 
            this.btn_Сохранить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Сохранить.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Сохранить.Location = new System.Drawing.Point(1372, 624);
            this.btn_Сохранить.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Сохранить.Name = "btn_Сохранить";
            this.btn_Сохранить.Size = new System.Drawing.Size(103, 36);
            this.btn_Сохранить.TabIndex = 21;
            this.btn_Сохранить.Text = "СОХР.";
            this.btn_Сохранить.UseVisualStyleBackColor = true;
            this.btn_Сохранить.Click += new System.EventHandler(this.btn_Сохранить_Click);
            // 
            // btn_ШаблонОповещения
            // 
            this.btn_ШаблонОповещения.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_ШаблонОповещения.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ШаблонОповещения.Location = new System.Drawing.Point(691, 623);
            this.btn_ШаблонОповещения.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ШаблонОповещения.Name = "btn_ШаблонОповещения";
            this.btn_ШаблонОповещения.Size = new System.Drawing.Size(463, 37);
            this.btn_ШаблонОповещения.TabIndex = 22;
            this.btn_ШаблонОповещения.Text = "Редактировать шаблон оповещения";
            this.btn_ШаблонОповещения.UseVisualStyleBackColor = true;
            this.btn_ШаблонОповещения.Click += new System.EventHandler(this.btn_ШаблонОповещения_Click);
            // 
            // gB_НумерацияПоезда
            // 
            this.gB_НумерацияПоезда.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_СХвоста);
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_СГоловы);
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_Отсутствует);
            this.gB_НумерацияПоезда.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_НумерацияПоезда.Location = new System.Drawing.Point(16, 591);
            this.gB_НумерацияПоезда.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_НумерацияПоезда.Name = "gB_НумерацияПоезда";
            this.gB_НумерацияПоезда.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_НумерацияПоезда.Size = new System.Drawing.Size(583, 66);
            this.gB_НумерацияПоезда.TabIndex = 23;
            this.gB_НумерацияПоезда.TabStop = false;
            this.gB_НумерацияПоезда.Text = "Нумерация поезда";
            // 
            // rB_Нумерация_СХвоста
            // 
            this.rB_Нумерация_СХвоста.AutoSize = true;
            this.rB_Нумерация_СХвоста.Location = new System.Drawing.Point(381, 28);
            this.rB_Нумерация_СХвоста.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rB_Нумерация_СХвоста.Name = "rB_Нумерация_СХвоста";
            this.rB_Нумерация_СХвоста.Size = new System.Drawing.Size(180, 32);
            this.rB_Нумерация_СХвоста.TabIndex = 3;
            this.rB_Нумерация_СХвоста.TabStop = true;
            this.rB_Нумерация_СХвоста.Text = "с хвоста состава";
            this.rB_Нумерация_СХвоста.UseVisualStyleBackColor = true;
            // 
            // rB_Нумерация_СГоловы
            // 
            this.rB_Нумерация_СГоловы.AutoSize = true;
            this.rB_Нумерация_СГоловы.Location = new System.Drawing.Point(176, 28);
            this.rB_Нумерация_СГоловы.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rB_Нумерация_СГоловы.Name = "rB_Нумерация_СГоловы";
            this.rB_Нумерация_СГоловы.Size = new System.Drawing.Size(189, 32);
            this.rB_Нумерация_СГоловы.TabIndex = 2;
            this.rB_Нумерация_СГоловы.TabStop = true;
            this.rB_Нумерация_СГоловы.Text = "с головы состава";
            this.rB_Нумерация_СГоловы.UseVisualStyleBackColor = true;
            // 
            // rB_Нумерация_Отсутствует
            // 
            this.rB_Нумерация_Отсутствует.AutoSize = true;
            this.rB_Нумерация_Отсутствует.Checked = true;
            this.rB_Нумерация_Отсутствует.Location = new System.Drawing.Point(20, 28);
            this.rB_Нумерация_Отсутствует.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rB_Нумерация_Отсутствует.Name = "rB_Нумерация_Отсутствует";
            this.rB_Нумерация_Отсутствует.Size = new System.Drawing.Size(136, 32);
            this.rB_Нумерация_Отсутствует.TabIndex = 1;
            this.rB_Нумерация_Отсутствует.TabStop = true;
            this.rB_Нумерация_Отсутствует.Text = "отсутствует";
            this.rB_Нумерация_Отсутствует.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(773, 540);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 28);
            this.label3.TabIndex = 24;
            this.label3.Text = "Номер пути";
            // 
            // cB_НомерПути
            // 
            this.cB_НомерПути.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cB_НомерПути.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cB_НомерПути.FormattingEnabled = true;
            this.cB_НомерПути.Items.AddRange(new object[] {
            "Неизвестно",
            "Первый путь",
            "Второй путь",
            "Третий путь",
            "Четвертый путь",
            "Пятый путь",
            "Шестой путь",
            "Седьмой путь",
            "Восьмой путь",
            "Девятый путь",
            "Десятый путь"});
            this.cB_НомерПути.Location = new System.Drawing.Point(899, 539);
            this.cB_НомерПути.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cB_НомерПути.Name = "cB_НомерПути";
            this.cB_НомерПути.Size = new System.Drawing.Size(255, 36);
            this.cB_НомерПути.TabIndex = 25;
            this.cB_НомерПути.Text = "Неизвестно";
            // 
            // gBОтображениеРасписания
            // 
            this.gBОтображениеРасписания.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gBОтображениеРасписания.Controls.Add(this.rBОтображениеПригород);
            this.gBОтображениеРасписания.Controls.Add(this.rBОтображениеДальнегоСледования);
            this.gBОтображениеРасписания.Controls.Add(this.rBОтображениеОтсутствует);
            this.gBОтображениеРасписания.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gBОтображениеРасписания.Location = new System.Drawing.Point(16, 656);
            this.gBОтображениеРасписания.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBОтображениеРасписания.Name = "gBОтображениеРасписания";
            this.gBОтображениеРасписания.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBОтображениеРасписания.Size = new System.Drawing.Size(583, 66);
            this.gBОтображениеРасписания.TabIndex = 24;
            this.gBОтображениеРасписания.TabStop = false;
            this.gBОтображениеРасписания.Text = "Отображение в окне расписания";
            this.gBОтображениеРасписания.CursorChanged += new System.EventHandler(this.gBОтображениеРасписания_CursorChanged);
            // 
            // rBОтображениеПригород
            // 
            this.rBОтображениеПригород.AutoSize = true;
            this.rBОтображениеПригород.Location = new System.Drawing.Point(381, 28);
            this.rBОтображениеПригород.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rBОтображениеПригород.Name = "rBОтображениеПригород";
            this.rBОтображениеПригород.Size = new System.Drawing.Size(124, 32);
            this.rBОтображениеПригород.TabIndex = 3;
            this.rBОтображениеПригород.TabStop = true;
            this.rBОтображениеПригород.Text = "пригород";
            this.rBОтображениеПригород.UseVisualStyleBackColor = true;
            // 
            // rBОтображениеДальнегоСледования
            // 
            this.rBОтображениеДальнегоСледования.AutoSize = true;
            this.rBОтображениеДальнегоСледования.Location = new System.Drawing.Point(176, 28);
            this.rBОтображениеДальнегоСледования.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rBОтображениеДальнегоСледования.Name = "rBОтображениеДальнегоСледования";
            this.rBОтображениеДальнегоСледования.Size = new System.Drawing.Size(157, 32);
            this.rBОтображениеДальнегоСледования.TabIndex = 2;
            this.rBОтображениеДальнегоСледования.TabStop = true;
            this.rBОтображениеДальнегоСледования.Text = "дальнее след.";
            this.rBОтображениеДальнегоСледования.UseVisualStyleBackColor = true;
            // 
            // rBОтображениеОтсутствует
            // 
            this.rBОтображениеОтсутствует.AutoSize = true;
            this.rBОтображениеОтсутствует.Checked = true;
            this.rBОтображениеОтсутствует.Location = new System.Drawing.Point(20, 28);
            this.rBОтображениеОтсутствует.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rBОтображениеОтсутствует.Name = "rBОтображениеОтсутствует";
            this.rBОтображениеОтсутствует.Size = new System.Drawing.Size(136, 32);
            this.rBОтображениеОтсутствует.TabIndex = 1;
            this.rBОтображениеОтсутствует.TabStop = true;
            this.rBОтображениеОтсутствует.Text = "отсутствует";
            this.rBОтображениеОтсутствует.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(624, 684);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 28);
            this.label5.TabIndex = 26;
            this.label5.Text = "Примечание";
            // 
            // lblСостояниеCIS
            // 
            this.lblСостояниеCIS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblСостояниеCIS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblСостояниеCIS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblСостояниеCIS.Location = new System.Drawing.Point(2, 0);
            this.lblСостояниеCIS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblСостояниеCIS.Name = "lblСостояниеCIS";
            this.lblСостояниеCIS.Size = new System.Drawing.Size(308, 55);
            this.lblСостояниеCIS.TabIndex = 28;
            this.lblСостояниеCIS.Text = "ЦИС НЕ на связи";
            this.lblСостояниеCIS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnСостояниеCIS
            // 
            this.pnСостояниеCIS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnСостояниеCIS.BackColor = System.Drawing.Color.Orange;
            this.pnСостояниеCIS.Controls.Add(this.lblСостояниеCIS);
            this.pnСостояниеCIS.Location = new System.Drawing.Point(1163, 667);
            this.pnСостояниеCIS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnСостояниеCIS.Name = "pnСостояниеCIS";
            this.pnСостояниеCIS.Size = new System.Drawing.Size(312, 55);
            this.pnСостояниеCIS.TabIndex = 29;
            // 
            // groupBoxSourseShedule
            // 
            this.groupBoxSourseShedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSourseShedule.Controls.Add(this.btnLoad);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleCis);
            this.groupBoxSourseShedule.Controls.Add(this.rbSourseSheduleLocal);
            this.groupBoxSourseShedule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSourseShedule.Location = new System.Drawing.Point(21, 467);
            this.groupBoxSourseShedule.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxSourseShedule.Name = "groupBoxSourseShedule";
            this.groupBoxSourseShedule.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxSourseShedule.Size = new System.Drawing.Size(743, 66);
            this.groupBoxSourseShedule.TabIndex = 25;
            this.groupBoxSourseShedule.TabStop = false;
            this.groupBoxSourseShedule.Text = "Источник загрузки расписания";
            // 
            // rbSourseSheduleCis
            // 
            this.rbSourseSheduleCis.AutoSize = true;
            this.rbSourseSheduleCis.Location = new System.Drawing.Point(176, 28);
            this.rbSourseSheduleCis.Margin = new System.Windows.Forms.Padding(4);
            this.rbSourseSheduleCis.Name = "rbSourseSheduleCis";
            this.rbSourseSheduleCis.Size = new System.Drawing.Size(75, 32);
            this.rbSourseSheduleCis.TabIndex = 2;
            this.rbSourseSheduleCis.TabStop = true;
            this.rbSourseSheduleCis.Text = "ЦИС";
            this.rbSourseSheduleCis.UseVisualStyleBackColor = true;
            // 
            // rbSourseSheduleLocal
            // 
            this.rbSourseSheduleLocal.AutoSize = true;
            this.rbSourseSheduleLocal.Checked = true;
            this.rbSourseSheduleLocal.Location = new System.Drawing.Point(20, 28);
            this.rbSourseSheduleLocal.Margin = new System.Windows.Forms.Padding(4);
            this.rbSourseSheduleLocal.Name = "rbSourseSheduleLocal";
            this.rbSourseSheduleLocal.Size = new System.Drawing.Size(135, 32);
            this.rbSourseSheduleLocal.TabIndex = 1;
            this.rbSourseSheduleLocal.TabStop = true;
            this.rbSourseSheduleLocal.Text = "локальный";
            this.rbSourseSheduleLocal.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(319, 22);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(392, 36);
            this.btnLoad.TabIndex = 30;
            this.btnLoad.Text = "Загрузить расписание";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // TrainTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 730);
            this.Controls.Add(this.groupBoxSourseShedule);
            this.Controls.Add(this.pnСостояниеCIS);
            this.Controls.Add(this.tBПримечание);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gBОтображениеРасписания);
            this.Controls.Add(this.cB_НомерПути);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gB_НумерацияПоезда);
            this.Controls.Add(this.btn_ШаблонОповещения);
            this.Controls.Add(this.btn_Сохранить);
            this.Controls.Add(this.btn_Вниз);
            this.Controls.Add(this.dTP_Отправление);
            this.Controls.Add(this.dTP_Прибытие);
            this.Controls.Add(this.btn_УдалитьЗапись);
            this.Controls.Add(this.btn_ИзменитьЗапись);
            this.Controls.Add(this.btn_Вверх);
            this.Controls.Add(this.btn_ДобавитьЗапись);
            this.Controls.Add(this.cB_Отправление);
            this.Controls.Add(this.cB_Прибытие);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tB_Название);
            this.Controls.Add(this.Название);
            this.Controls.Add(this.tB_Номер);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::MainExample.Properties.Resources.SmallIcon;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TrainTable";
            this.Text = "Расписание движения поездов";
            this.gB_НумерацияПоезда.ResumeLayout(false);
            this.gB_НумерацияПоезда.PerformLayout();
            this.gBОтображениеРасписания.ResumeLayout(false);
            this.gBОтображениеРасписания.PerformLayout();
            this.pnСостояниеCIS.ResumeLayout(false);
            this.groupBoxSourseShedule.ResumeLayout(false);
            this.groupBoxSourseShedule.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tB_Номер;
        private System.Windows.Forms.TextBox tB_Название;
        private System.Windows.Forms.Label Название;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cB_Прибытие;
        private System.Windows.Forms.CheckBox cB_Отправление;
        private System.Windows.Forms.Button btn_ДобавитьЗапись;
        private System.Windows.Forms.Button btn_ИзменитьЗапись;
        private System.Windows.Forms.Button btn_УдалитьЗапись;
        private System.Windows.Forms.DateTimePicker dTP_Прибытие;
        private System.Windows.Forms.DateTimePicker dTP_Отправление;
        private System.Windows.Forms.Button btn_Вверх;
        private System.Windows.Forms.Button btn_Вниз;
        private System.Windows.Forms.Button btn_Сохранить;
        private System.Windows.Forms.Button btn_ШаблонОповещения;
        private System.Windows.Forms.GroupBox gB_НумерацияПоезда;
        private System.Windows.Forms.RadioButton rB_Нумерация_СХвоста;
        private System.Windows.Forms.RadioButton rB_Нумерация_СГоловы;
        private System.Windows.Forms.RadioButton rB_Нумерация_Отсутствует;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cB_НомерПути;
        private System.Windows.Forms.GroupBox gBОтображениеРасписания;
        private System.Windows.Forms.RadioButton rBОтображениеПригород;
        private System.Windows.Forms.RadioButton rBОтображениеДальнегоСледования;
        private System.Windows.Forms.RadioButton rBОтображениеОтсутствует;
        private System.Windows.Forms.TextBox tBПримечание;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblСостояниеCIS;
        private System.Windows.Forms.Panel pnСостояниеCIS;
        private System.Windows.Forms.GroupBox groupBoxSourseShedule;
        private System.Windows.Forms.RadioButton rbSourseSheduleCis;
        private System.Windows.Forms.RadioButton rbSourseSheduleLocal;
        private System.Windows.Forms.Button btnLoad;
    }
}