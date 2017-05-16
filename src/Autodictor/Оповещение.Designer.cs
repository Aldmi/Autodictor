namespace MainExample
{
    partial class Оповещение
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
            this.btn_Принять = new System.Windows.Forms.Button();
            this.btn_Отменить = new System.Windows.Forms.Button();
            this.gBНаправление = new System.Windows.Forms.GroupBox();
            this.dTPПрибытие = new System.Windows.Forms.DateTimePicker();
            this.cBКуда = new System.Windows.Forms.ComboBox();
            this.dTPОтправление = new System.Windows.Forms.DateTimePicker();
            this.lКуда = new System.Windows.Forms.Label();
            this.cBОткуда = new System.Windows.Forms.ComboBox();
            this.lОткуда = new System.Windows.Forms.Label();
            this.rBТранзит = new System.Windows.Forms.RadioButton();
            this.rBОтправление = new System.Windows.Forms.RadioButton();
            this.rBПрибытие = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tBНомерПоезда = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cBКатегория = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cBПутьПоУмолчанию = new System.Windows.Forms.ComboBox();
            this.cBОтсчетВагонов = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gBОстановки = new System.Windows.Forms.GroupBox();
            this.btnРедактировать = new System.Windows.Forms.Button();
            this.lVСписокСтанций = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rBСОстановкамиКроме = new System.Windows.Forms.RadioButton();
            this.rBСОстановкамиНа = new System.Windows.Forms.RadioButton();
            this.rBБезОстановок = new System.Windows.Forms.RadioButton();
            this.rBСоВсемиОстановками = new System.Windows.Forms.RadioButton();
            this.rBНеОповещать = new System.Windows.Forms.RadioButton();
            this.gBДниСледования = new System.Windows.Forms.GroupBox();
            this.tBОписаниеДнейСледования = new System.Windows.Forms.TextBox();
            this.btnДниСледования = new System.Windows.Forms.Button();
            this.rBВремяДействияПостоянно = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.dTPВремяДействияПо2 = new System.Windows.Forms.DateTimePicker();
            this.dTPВремяДействияС2 = new System.Windows.Forms.DateTimePicker();
            this.dTPВремяДействияПо = new System.Windows.Forms.DateTimePicker();
            this.dTPВремяДействияС = new System.Windows.Forms.DateTimePicker();
            this.rBВремяДействияСПо = new System.Windows.Forms.RadioButton();
            this.rBВремяДействияПо = new System.Windows.Forms.RadioButton();
            this.rBВремяДействияС = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.cBБлокировка = new System.Windows.Forms.CheckBox();
            this.gBШаблонОповещения = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cBВремяОповещения = new System.Windows.Forms.ComboBox();
            this.tBВремяОповещения = new System.Windows.Forms.TextBox();
            this.cBШаблонОповещения = new System.Windows.Forms.ComboBox();
            this.lVШаблоныОповещения = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnУдалитьШаблон = new System.Windows.Forms.Button();
            this.btnДобавитьШаблон = new System.Windows.Forms.Button();
            this.tb_Дополнение = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gBНаправление.SuspendLayout();
            this.gBОстановки.SuspendLayout();
            this.gBДниСледования.SuspendLayout();
            this.gBШаблонОповещения.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Принять
            // 
            this.btn_Принять.Location = new System.Drawing.Point(319, 756);
            this.btn_Принять.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btn_Принять.Name = "btn_Принять";
            this.btn_Принять.Size = new System.Drawing.Size(123, 35);
            this.btn_Принять.TabIndex = 33;
            this.btn_Принять.Text = "Принять";
            this.btn_Принять.UseVisualStyleBackColor = true;
            this.btn_Принять.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Отменить
            // 
            this.btn_Отменить.Location = new System.Drawing.Point(448, 756);
            this.btn_Отменить.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btn_Отменить.Name = "btn_Отменить";
            this.btn_Отменить.Size = new System.Drawing.Size(123, 35);
            this.btn_Отменить.TabIndex = 34;
            this.btn_Отменить.Text = "Отменить";
            this.btn_Отменить.UseVisualStyleBackColor = true;
            this.btn_Отменить.Click += new System.EventHandler(this.button2_Click);
            // 
            // gBНаправление
            // 
            this.gBНаправление.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.gBНаправление.Controls.Add(this.dTPПрибытие);
            this.gBНаправление.Controls.Add(this.cBКуда);
            this.gBНаправление.Controls.Add(this.dTPОтправление);
            this.gBНаправление.Controls.Add(this.lКуда);
            this.gBНаправление.Controls.Add(this.cBОткуда);
            this.gBНаправление.Controls.Add(this.lОткуда);
            this.gBНаправление.Controls.Add(this.rBТранзит);
            this.gBНаправление.Controls.Add(this.rBОтправление);
            this.gBНаправление.Controls.Add(this.rBПрибытие);
            this.gBНаправление.Location = new System.Drawing.Point(14, 111);
            this.gBНаправление.Margin = new System.Windows.Forms.Padding(5);
            this.gBНаправление.Name = "gBНаправление";
            this.gBНаправление.Padding = new System.Windows.Forms.Padding(5);
            this.gBНаправление.Size = new System.Drawing.Size(546, 154);
            this.gBНаправление.TabIndex = 35;
            this.gBНаправление.TabStop = false;
            this.gBНаправление.Text = "Направление";
            // 
            // dTPПрибытие
            // 
            this.dTPПрибытие.CustomFormat = "HH:mm";
            this.dTPПрибытие.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPПрибытие.Location = new System.Drawing.Point(466, 106);
            this.dTPПрибытие.Name = "dTPПрибытие";
            this.dTPПрибытие.ShowUpDown = true;
            this.dTPПрибытие.Size = new System.Drawing.Size(72, 26);
            this.dTPПрибытие.TabIndex = 43;
            // 
            // cBКуда
            // 
            this.cBКуда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBКуда.FormattingEnabled = true;
            this.cBКуда.Location = new System.Drawing.Point(91, 105);
            this.cBКуда.Name = "cBКуда";
            this.cBКуда.Size = new System.Drawing.Size(369, 28);
            this.cBКуда.TabIndex = 41;
            // 
            // dTPОтправление
            // 
            this.dTPОтправление.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPОтправление.CustomFormat = "HH:mm";
            this.dTPОтправление.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPОтправление.Location = new System.Drawing.Point(466, 72);
            this.dTPОтправление.Name = "dTPОтправление";
            this.dTPОтправление.ShowUpDown = true;
            this.dTPОтправление.Size = new System.Drawing.Size(72, 26);
            this.dTPОтправление.TabIndex = 42;
            // 
            // lКуда
            // 
            this.lКуда.AutoSize = true;
            this.lКуда.Location = new System.Drawing.Point(14, 108);
            this.lКуда.Name = "lКуда";
            this.lКуда.Size = new System.Drawing.Size(50, 20);
            this.lКуда.TabIndex = 40;
            this.lКуда.Text = "Куда";
            // 
            // cBОткуда
            // 
            this.cBОткуда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBОткуда.FormattingEnabled = true;
            this.cBОткуда.Location = new System.Drawing.Point(91, 71);
            this.cBОткуда.Name = "cBОткуда";
            this.cBОткуда.Size = new System.Drawing.Size(369, 28);
            this.cBОткуда.TabIndex = 39;
            // 
            // lОткуда
            // 
            this.lОткуда.AutoSize = true;
            this.lОткуда.Location = new System.Drawing.Point(14, 74);
            this.lОткуда.Name = "lОткуда";
            this.lОткуда.Size = new System.Drawing.Size(71, 20);
            this.lОткуда.TabIndex = 38;
            this.lОткуда.Text = "Откуда";
            // 
            // rBТранзит
            // 
            this.rBТранзит.AutoSize = true;
            this.rBТранзит.Checked = true;
            this.rBТранзит.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBТранзит.Location = new System.Drawing.Point(423, 28);
            this.rBТранзит.Name = "rBТранзит";
            this.rBТранзит.Size = new System.Drawing.Size(89, 24);
            this.rBТранзит.TabIndex = 2;
            this.rBТранзит.TabStop = true;
            this.rBТранзит.Text = "Транзит";
            this.rBТранзит.UseVisualStyleBackColor = true;
            this.rBТранзит.CheckedChanged += new System.EventHandler(this.rBОтправление_CheckedChanged);
            // 
            // rBОтправление
            // 
            this.rBОтправление.AutoSize = true;
            this.rBОтправление.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBОтправление.Location = new System.Drawing.Point(193, 28);
            this.rBОтправление.Name = "rBОтправление";
            this.rBОтправление.Size = new System.Drawing.Size(130, 24);
            this.rBОтправление.TabIndex = 1;
            this.rBОтправление.Text = "Отправление";
            this.rBОтправление.UseVisualStyleBackColor = true;
            this.rBОтправление.CheckedChanged += new System.EventHandler(this.rBОтправление_CheckedChanged);
            // 
            // rBПрибытие
            // 
            this.rBПрибытие.AutoSize = true;
            this.rBПрибытие.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBПрибытие.Location = new System.Drawing.Point(9, 28);
            this.rBПрибытие.Name = "rBПрибытие";
            this.rBПрибытие.Size = new System.Drawing.Size(104, 24);
            this.rBПрибытие.TabIndex = 0;
            this.rBПрибытие.Text = "Прибытие";
            this.rBПрибытие.UseVisualStyleBackColor = true;
            this.rBПрибытие.CheckedChanged += new System.EventHandler(this.rBОтправление_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "Поезд №";
            // 
            // tBНомерПоезда
            // 
            this.tBНомерПоезда.Location = new System.Drawing.Point(103, 18);
            this.tBНомерПоезда.Name = "tBНомерПоезда";
            this.tBНомерПоезда.Size = new System.Drawing.Size(135, 26);
            this.tBНомерПоезда.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 44;
            this.label2.Text = "Категория";
            // 
            // cBКатегория
            // 
            this.cBКатегория.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBКатегория.FormattingEnabled = true;
            this.cBКатегория.Items.AddRange(new object[] {
            "НеОпределен",
            "Пассажирский",
            "Пригородный",
            "Фирменный",
            "Скорый",
            "Скоростной",
            "Ласточка",
            "РЭКС"});
            this.cBКатегория.Location = new System.Drawing.Point(354, 19);
            this.cBКатегория.Name = "cBКатегория";
            this.cBКатегория.Size = new System.Drawing.Size(206, 28);
            this.cBКатегория.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 20);
            this.label3.TabIndex = 45;
            this.label3.Text = "Путь по умолчанию";
            // 
            // cBПутьПоУмолчанию
            // 
            this.cBПутьПоУмолчанию.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBПутьПоУмолчанию.FormattingEnabled = true;
            this.cBПутьПоУмолчанию.Location = new System.Drawing.Point(219, 276);
            this.cBПутьПоУмолчанию.Name = "cBПутьПоУмолчанию";
            this.cBПутьПоУмолчанию.Size = new System.Drawing.Size(341, 28);
            this.cBПутьПоУмолчанию.TabIndex = 46;
            // 
            // cBОтсчетВагонов
            // 
            this.cBОтсчетВагонов.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBОтсчетВагонов.FormattingEnabled = true;
            this.cBОтсчетВагонов.Items.AddRange(new object[] {
            "Не объявлять",
            "С головы состава",
            "С хвоста состава"});
            this.cBОтсчетВагонов.Location = new System.Drawing.Point(219, 316);
            this.cBОтсчетВагонов.Name = "cBОтсчетВагонов";
            this.cBОтсчетВагонов.Size = new System.Drawing.Size(341, 28);
            this.cBОтсчетВагонов.TabIndex = 48;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 319);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 20);
            this.label4.TabIndex = 47;
            this.label4.Text = "Нумерация вагонов";
            // 
            // gBОстановки
            // 
            this.gBОстановки.Controls.Add(this.btnРедактировать);
            this.gBОстановки.Controls.Add(this.lVСписокСтанций);
            this.gBОстановки.Controls.Add(this.rBСОстановкамиКроме);
            this.gBОстановки.Controls.Add(this.rBСОстановкамиНа);
            this.gBОстановки.Controls.Add(this.rBБезОстановок);
            this.gBОстановки.Controls.Add(this.rBСоВсемиОстановками);
            this.gBОстановки.Controls.Add(this.rBНеОповещать);
            this.gBОстановки.Location = new System.Drawing.Point(581, 11);
            this.gBОстановки.Name = "gBОстановки";
            this.gBОстановки.Size = new System.Drawing.Size(585, 333);
            this.gBОстановки.TabIndex = 49;
            this.gBОстановки.TabStop = false;
            this.gBОстановки.Text = "Остановки";
            // 
            // btnРедактировать
            // 
            this.btnРедактировать.Location = new System.Drawing.Point(15, 278);
            this.btnРедактировать.Name = "btnРедактировать";
            this.btnРедактировать.Size = new System.Drawing.Size(186, 45);
            this.btnРедактировать.TabIndex = 50;
            this.btnРедактировать.Text = "Редактировать";
            this.btnРедактировать.UseVisualStyleBackColor = true;
            this.btnРедактировать.Click += new System.EventHandler(this.btnРедактировать_Click);
            // 
            // lVСписокСтанций
            // 
            this.lVСписокСтанций.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lVСписокСтанций.FullRowSelect = true;
            this.lVСписокСтанций.GridLines = true;
            this.lVСписокСтанций.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lVСписокСтанций.Location = new System.Drawing.Point(213, 34);
            this.lVСписокСтанций.Name = "lVСписокСтанций";
            this.lVСписокСтанций.Size = new System.Drawing.Size(366, 293);
            this.lVСписокСтанций.TabIndex = 49;
            this.lVСписокСтанций.UseCompatibleStateImageBehavior = false;
            this.lVСписокСтанций.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 360;
            // 
            // rBСОстановкамиКроме
            // 
            this.rBСОстановкамиКроме.AutoSize = true;
            this.rBСОстановкамиКроме.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBСОстановкамиКроме.Location = new System.Drawing.Point(6, 94);
            this.rBСОстановкамиКроме.Name = "rBСОстановкамиКроме";
            this.rBСОстановкамиКроме.Size = new System.Drawing.Size(195, 24);
            this.rBСОстановкамиКроме.TabIndex = 48;
            this.rBСОстановкамиКроме.Text = "С остановками кроме:";
            this.rBСОстановкамиКроме.UseVisualStyleBackColor = true;
            // 
            // rBСОстановкамиНа
            // 
            this.rBСОстановкамиНа.AutoSize = true;
            this.rBСОстановкамиНа.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBСОстановкамиНа.Location = new System.Drawing.Point(6, 64);
            this.rBСОстановкамиНа.Name = "rBСОстановкамиНа";
            this.rBСОстановкамиНа.Size = new System.Drawing.Size(167, 24);
            this.rBСОстановкамиНа.TabIndex = 47;
            this.rBСОстановкамиНа.Text = "С остановками на:";
            this.rBСОстановкамиНа.UseVisualStyleBackColor = true;
            // 
            // rBБезОстановок
            // 
            this.rBБезОстановок.AutoSize = true;
            this.rBБезОстановок.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBБезОстановок.Location = new System.Drawing.Point(6, 154);
            this.rBБезОстановок.Name = "rBБезОстановок";
            this.rBБезОстановок.Size = new System.Drawing.Size(138, 24);
            this.rBБезОстановок.TabIndex = 46;
            this.rBБезОстановок.Text = "Без остановок";
            this.rBБезОстановок.UseVisualStyleBackColor = true;
            // 
            // rBСоВсемиОстановками
            // 
            this.rBСоВсемиОстановками.AutoSize = true;
            this.rBСоВсемиОстановками.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBСоВсемиОстановками.Location = new System.Drawing.Point(7, 124);
            this.rBСоВсемиОстановками.Name = "rBСоВсемиОстановками";
            this.rBСоВсемиОстановками.Size = new System.Drawing.Size(200, 24);
            this.rBСоВсемиОстановками.TabIndex = 45;
            this.rBСоВсемиОстановками.Text = "Со всеми остановками";
            this.rBСоВсемиОстановками.UseVisualStyleBackColor = true;
            // 
            // rBНеОповещать
            // 
            this.rBНеОповещать.AutoSize = true;
            this.rBНеОповещать.Checked = true;
            this.rBНеОповещать.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBНеОповещать.Location = new System.Drawing.Point(6, 34);
            this.rBНеОповещать.Name = "rBНеОповещать";
            this.rBНеОповещать.Size = new System.Drawing.Size(137, 24);
            this.rBНеОповещать.TabIndex = 44;
            this.rBНеОповещать.TabStop = true;
            this.rBНеОповещать.Text = "Не оповещать";
            this.rBНеОповещать.UseVisualStyleBackColor = true;
            // 
            // gBДниСледования
            // 
            this.gBДниСледования.Controls.Add(this.tBОписаниеДнейСледования);
            this.gBДниСледования.Controls.Add(this.btnДниСледования);
            this.gBДниСледования.Controls.Add(this.rBВремяДействияПостоянно);
            this.gBДниСледования.Controls.Add(this.label6);
            this.gBДниСледования.Controls.Add(this.dTPВремяДействияПо2);
            this.gBДниСледования.Controls.Add(this.dTPВремяДействияС2);
            this.gBДниСледования.Controls.Add(this.dTPВремяДействияПо);
            this.gBДниСледования.Controls.Add(this.dTPВремяДействияС);
            this.gBДниСледования.Controls.Add(this.rBВремяДействияСПо);
            this.gBДниСледования.Controls.Add(this.rBВремяДействияПо);
            this.gBДниСледования.Controls.Add(this.rBВремяДействияС);
            this.gBДниСледования.Controls.Add(this.label5);
            this.gBДниСледования.Location = new System.Drawing.Point(14, 361);
            this.gBДниСледования.Name = "gBДниСледования";
            this.gBДниСледования.Size = new System.Drawing.Size(378, 375);
            this.gBДниСледования.TabIndex = 50;
            this.gBДниСледования.TabStop = false;
            this.gBДниСледования.Text = "Дни следования";
            // 
            // tBОписаниеДнейСледования
            // 
            this.tBОписаниеДнейСледования.Location = new System.Drawing.Point(18, 251);
            this.tBОписаниеДнейСледования.Multiline = true;
            this.tBОписаниеДнейСледования.Name = "tBОписаниеДнейСледования";
            this.tBОписаниеДнейСледования.ReadOnly = true;
            this.tBОписаниеДнейСледования.Size = new System.Drawing.Size(349, 118);
            this.tBОписаниеДнейСледования.TabIndex = 59;
            // 
            // btnДниСледования
            // 
            this.btnДниСледования.Location = new System.Drawing.Point(18, 200);
            this.btnДниСледования.Name = "btnДниСледования";
            this.btnДниСледования.Size = new System.Drawing.Size(349, 45);
            this.btnДниСледования.TabIndex = 51;
            this.btnДниСледования.Text = "Дни следования";
            this.btnДниСледования.UseVisualStyleBackColor = true;
            this.btnДниСледования.Click += new System.EventHandler(this.btnДниСледования_Click);
            // 
            // rBВремяДействияПостоянно
            // 
            this.rBВремяДействияПостоянно.AutoSize = true;
            this.rBВремяДействияПостоянно.Checked = true;
            this.rBВремяДействияПостоянно.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияПостоянно.Location = new System.Drawing.Point(18, 151);
            this.rBВремяДействияПостоянно.Name = "rBВремяДействияПостоянно";
            this.rBВремяДействияПостоянно.Size = new System.Drawing.Size(110, 24);
            this.rBВремяДействияПостоянно.TabIndex = 58;
            this.rBВремяДействияПостоянно.TabStop = true;
            this.rBВремяДействияПостоянно.Text = "Постоянно";
            this.rBВремяДействияПостоянно.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(207, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 20);
            this.label6.TabIndex = 57;
            this.label6.Text = "по";
            // 
            // dTPВремяДействияПо2
            // 
            this.dTPВремяДействияПо2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияПо2.CustomFormat = "HH:mm";
            this.dTPВремяДействияПо2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияПо2.Location = new System.Drawing.Point(240, 123);
            this.dTPВремяДействияПо2.Name = "dTPВремяДействияПо2";
            this.dTPВремяДействияПо2.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияПо2.TabIndex = 56;
            // 
            // dTPВремяДействияС2
            // 
            this.dTPВремяДействияС2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияС2.CustomFormat = "HH:mm";
            this.dTPВремяДействияС2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияС2.Location = new System.Drawing.Point(70, 123);
            this.dTPВремяДействияС2.Name = "dTPВремяДействияС2";
            this.dTPВремяДействияС2.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияС2.TabIndex = 55;
            // 
            // dTPВремяДействияПо
            // 
            this.dTPВремяДействияПо.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияПо.CustomFormat = "HH:mm";
            this.dTPВремяДействияПо.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияПо.Location = new System.Drawing.Point(70, 91);
            this.dTPВремяДействияПо.Name = "dTPВремяДействияПо";
            this.dTPВремяДействияПо.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияПо.TabIndex = 54;
            // 
            // dTPВремяДействияС
            // 
            this.dTPВремяДействияС.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dTPВремяДействияС.CustomFormat = "HH:mm";
            this.dTPВремяДействияС.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPВремяДействияС.Location = new System.Drawing.Point(70, 61);
            this.dTPВремяДействияС.Name = "dTPВремяДействияС";
            this.dTPВремяДействияС.Size = new System.Drawing.Size(127, 26);
            this.dTPВремяДействияС.TabIndex = 44;
            // 
            // rBВремяДействияСПо
            // 
            this.rBВремяДействияСПо.AutoSize = true;
            this.rBВремяДействияСПо.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияСПо.Location = new System.Drawing.Point(18, 121);
            this.rBВремяДействияСПо.Name = "rBВремяДействияСПо";
            this.rBВремяДействияСПо.Size = new System.Drawing.Size(35, 24);
            this.rBВремяДействияСПо.TabIndex = 53;
            this.rBВремяДействияСПо.Text = "с";
            this.rBВремяДействияСПо.UseVisualStyleBackColor = true;
            // 
            // rBВремяДействияПо
            // 
            this.rBВремяДействияПо.AutoSize = true;
            this.rBВремяДействияПо.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияПо.Location = new System.Drawing.Point(18, 91);
            this.rBВремяДействияПо.Name = "rBВремяДействияПо";
            this.rBВремяДействияПо.Size = new System.Drawing.Size(45, 24);
            this.rBВремяДействияПо.TabIndex = 52;
            this.rBВремяДействияПо.Text = "по";
            this.rBВремяДействияПо.UseVisualStyleBackColor = true;
            // 
            // rBВремяДействияС
            // 
            this.rBВремяДействияС.AutoSize = true;
            this.rBВремяДействияС.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rBВремяДействияС.Location = new System.Drawing.Point(18, 61);
            this.rBВремяДействияС.Name = "rBВремяДействияС";
            this.rBВремяДействияС.Size = new System.Drawing.Size(35, 24);
            this.rBВремяДействияС.TabIndex = 44;
            this.rBВремяДействияС.Text = "с";
            this.rBВремяДействияС.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(312, 20);
            this.label5.TabIndex = 51;
            this.label5.Text = "Общее время действия расписания";
            // 
            // cBБлокировка
            // 
            this.cBБлокировка.AutoSize = true;
            this.cBБлокировка.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBБлокировка.ForeColor = System.Drawing.Color.OrangeRed;
            this.cBБлокировка.Location = new System.Drawing.Point(20, 755);
            this.cBБлокировка.Name = "cBБлокировка";
            this.cBБлокировка.Size = new System.Drawing.Size(275, 33);
            this.cBБлокировка.TabIndex = 51;
            this.cBБлокировка.Text = "Блокировка поезда";
            this.cBБлокировка.UseVisualStyleBackColor = true;
            this.cBБлокировка.CheckedChanged += new System.EventHandler(this.cBБлокировка_CheckedChanged);
            // 
            // gBШаблонОповещения
            // 
            this.gBШаблонОповещения.Controls.Add(this.label7);
            this.gBШаблонОповещения.Controls.Add(this.cBВремяОповещения);
            this.gBШаблонОповещения.Controls.Add(this.tBВремяОповещения);
            this.gBШаблонОповещения.Controls.Add(this.cBШаблонОповещения);
            this.gBШаблонОповещения.Controls.Add(this.lVШаблоныОповещения);
            this.gBШаблонОповещения.Controls.Add(this.btnУдалитьШаблон);
            this.gBШаблонОповещения.Controls.Add(this.btnДобавитьШаблон);
            this.gBШаблонОповещения.Location = new System.Drawing.Point(398, 361);
            this.gBШаблонОповещения.Name = "gBШаблонОповещения";
            this.gBШаблонОповещения.Size = new System.Drawing.Size(768, 375);
            this.gBШаблонОповещения.TabIndex = 52;
            this.gBШаблонОповещения.TabStop = false;
            this.gBШаблонОповещения.Text = "Шаблоны оповещения";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(192, 20);
            this.label7.TabIndex = 53;
            this.label7.Text = "Время через запятую";
            // 
            // cBВремяОповещения
            // 
            this.cBВремяОповещения.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBВремяОповещения.FormattingEnabled = true;
            this.cBВремяОповещения.Items.AddRange(new object[] {
            "Прибытие",
            "Отправление"});
            this.cBВремяОповещения.Location = new System.Drawing.Point(479, 56);
            this.cBВремяОповещения.Name = "cBВремяОповещения";
            this.cBВремяОповещения.Size = new System.Drawing.Size(283, 28);
            this.cBВремяОповещения.TabIndex = 57;
            // 
            // tBВремяОповещения
            // 
            this.tBВремяОповещения.Location = new System.Drawing.Point(204, 56);
            this.tBВремяОповещения.Name = "tBВремяОповещения";
            this.tBВремяОповещения.Size = new System.Drawing.Size(176, 26);
            this.tBВремяОповещения.TabIndex = 55;
            // 
            // cBШаблонОповещения
            // 
            this.cBШаблонОповещения.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBШаблонОповещения.FormattingEnabled = true;
            this.cBШаблонОповещения.Location = new System.Drawing.Point(6, 23);
            this.cBШаблонОповещения.Name = "cBШаблонОповещения";
            this.cBШаблонОповещения.Size = new System.Drawing.Size(756, 28);
            this.cBШаблонОповещения.TabIndex = 53;
            // 
            // lVШаблоныОповещения
            // 
            this.lVШаблоныОповещения.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5});
            this.lVШаблоныОповещения.FullRowSelect = true;
            this.lVШаблоныОповещения.GridLines = true;
            this.lVШаблоныОповещения.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lVШаблоныОповещения.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lVШаблоныОповещения.Location = new System.Drawing.Point(117, 91);
            this.lVШаблоныОповещения.Name = "lVШаблоныОповещения";
            this.lVШаблоныОповещения.Size = new System.Drawing.Size(645, 278);
            this.lVШаблоныОповещения.TabIndex = 51;
            this.lVШаблоныОповещения.UseCompatibleStateImageBehavior = false;
            this.lVШаблоныОповещения.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Шаблон";
            this.columnHeader2.Width = 340;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Время оповещения";
            this.columnHeader3.Width = 175;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Время";
            this.columnHeader5.Width = 125;
            // 
            // btnУдалитьШаблон
            // 
            this.btnУдалитьШаблон.Location = new System.Drawing.Point(6, 140);
            this.btnУдалитьШаблон.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnУдалитьШаблон.Name = "btnУдалитьШаблон";
            this.btnУдалитьШаблон.Size = new System.Drawing.Size(105, 35);
            this.btnУдалитьШаблон.TabIndex = 54;
            this.btnУдалитьШаблон.Text = "Удалить";
            this.btnУдалитьШаблон.UseVisualStyleBackColor = true;
            this.btnУдалитьШаблон.Click += new System.EventHandler(this.btnУдалитьШаблон_Click);
            // 
            // btnДобавитьШаблон
            // 
            this.btnДобавитьШаблон.Location = new System.Drawing.Point(6, 95);
            this.btnДобавитьШаблон.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnДобавитьШаблон.Name = "btnДобавитьШаблон";
            this.btnДобавитьШаблон.Size = new System.Drawing.Size(105, 35);
            this.btnДобавитьШаблон.TabIndex = 53;
            this.btnДобавитьШаблон.Text = "Добавить";
            this.btnДобавитьШаблон.UseVisualStyleBackColor = true;
            this.btnДобавитьШаблон.Click += new System.EventHandler(this.btnДобавитьШаблон_Click);
            // 
            // tb_Дополнение
            // 
            this.tb_Дополнение.Location = new System.Drawing.Point(131, 64);
            this.tb_Дополнение.Name = "tb_Дополнение";
            this.tb_Дополнение.Size = new System.Drawing.Size(429, 26);
            this.tb_Дополнение.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 20);
            this.label8.TabIndex = 53;
            this.label8.Text = "Дополнение";
            // 
            // Оповещение
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 806);
            this.Controls.Add(this.tb_Дополнение);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gBШаблонОповещения);
            this.Controls.Add(this.cBБлокировка);
            this.Controls.Add(this.gBДниСледования);
            this.Controls.Add(this.gBОстановки);
            this.Controls.Add(this.cBОтсчетВагонов);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cBПутьПоУмолчанию);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cBКатегория);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBНомерПоезда);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gBНаправление);
            this.Controls.Add(this.btn_Отменить);
            this.Controls.Add(this.btn_Принять);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "Оповещение";
            this.Text = "Редактор расписания движения для поезда";
            this.gBНаправление.ResumeLayout(false);
            this.gBНаправление.PerformLayout();
            this.gBОстановки.ResumeLayout(false);
            this.gBОстановки.PerformLayout();
            this.gBДниСледования.ResumeLayout(false);
            this.gBДниСледования.PerformLayout();
            this.gBШаблонОповещения.ResumeLayout(false);
            this.gBШаблонОповещения.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_Принять;
        private System.Windows.Forms.Button btn_Отменить;
        private System.Windows.Forms.GroupBox gBНаправление;
        private System.Windows.Forms.DateTimePicker dTPПрибытие;
        private System.Windows.Forms.ComboBox cBКуда;
        private System.Windows.Forms.Label lКуда;
        private System.Windows.Forms.ComboBox cBОткуда;
        private System.Windows.Forms.Label lОткуда;
        private System.Windows.Forms.RadioButton rBТранзит;
        private System.Windows.Forms.RadioButton rBОтправление;
        private System.Windows.Forms.RadioButton rBПрибытие;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBНомерПоезда;
        private System.Windows.Forms.DateTimePicker dTPОтправление;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cBКатегория;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBПутьПоУмолчанию;
        private System.Windows.Forms.ComboBox cBОтсчетВагонов;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gBОстановки;
        private System.Windows.Forms.ListView lVСписокСтанций;
        private System.Windows.Forms.RadioButton rBСОстановкамиКроме;
        private System.Windows.Forms.RadioButton rBСОстановкамиНа;
        private System.Windows.Forms.RadioButton rBБезОстановок;
        private System.Windows.Forms.RadioButton rBСоВсемиОстановками;
        private System.Windows.Forms.RadioButton rBНеОповещать;
        private System.Windows.Forms.Button btnРедактировать;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox gBДниСледования;
        private System.Windows.Forms.RadioButton rBВремяДействияПостоянно;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияПо2;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияС2;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияПо;
        private System.Windows.Forms.DateTimePicker dTPВремяДействияС;
        private System.Windows.Forms.RadioButton rBВремяДействияСПо;
        private System.Windows.Forms.RadioButton rBВремяДействияПо;
        private System.Windows.Forms.RadioButton rBВремяДействияС;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox cBБлокировка;
        private System.Windows.Forms.Button btnДниСледования;
        private System.Windows.Forms.TextBox tBОписаниеДнейСледования;
        private System.Windows.Forms.GroupBox gBШаблонОповещения;
        private System.Windows.Forms.TextBox tBВремяОповещения;
        private System.Windows.Forms.ComboBox cBШаблонОповещения;
        private System.Windows.Forms.ListView lVШаблоныОповещения;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnУдалитьШаблон;
        private System.Windows.Forms.Button btnДобавитьШаблон;
        private System.Windows.Forms.ComboBox cBВремяОповещения;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Дополнение;
        private System.Windows.Forms.Label label8;
    }
}