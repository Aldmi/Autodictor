namespace MainExample
{
    partial class КарточкаДвиженияПоезда
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
            this.gB_НумерацияПоезда = new System.Windows.Forms.GroupBox();
            this.rB_Нумерация_СХвоста = new System.Windows.Forms.RadioButton();
            this.rB_Нумерация_СГоловы = new System.Windows.Forms.RadioButton();
            this.rB_Нумерация_Отсутствует = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lVШаблоны = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rTB_Сообщение = new System.Windows.Forms.RichTextBox();
            this.btnВоспроизвестиВыбранныйШаблон = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cB_НомерПути = new System.Windows.Forms.ComboBox();
            this.btn_Подтвердить = new System.Windows.Forms.Button();
            this.gB_Прибытие = new System.Windows.Forms.GroupBox();
            this.cBОтправление = new System.Windows.Forms.CheckBox();
            this.cBПрибытие = new System.Windows.Forms.CheckBox();
            this.btn_ИзменитьВремяОтправления = new System.Windows.Forms.Button();
            this.dTP_ВремяОтправления = new System.Windows.Forms.DateTimePicker();
            this.btn_ИзменитьВремяПрибытия = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dTP_Прибытие = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox_displayTable = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rB_СоВсемиОстановками = new System.Windows.Forms.RadioButton();
            this.btnРедактировать = new System.Windows.Forms.Button();
            this.lB_ПоСтанциям = new System.Windows.Forms.ListBox();
            this.rB_КромеСтанций = new System.Windows.Forms.RadioButton();
            this.rB_ПоСтанциям = new System.Windows.Forms.RadioButton();
            this.rB_ПоРасписанию = new System.Windows.Forms.RadioButton();
            this.cBКуда = new System.Windows.Forms.ComboBox();
            this.cBОткуда = new System.Windows.Forms.ComboBox();
            this.cBНомерПоезда = new System.Windows.Forms.ComboBox();
            this.btnПовторения = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cBОтменен = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnЗадержкаОтправления = new System.Windows.Forms.Button();
            this.btnЗадержкаПрибытия = new System.Windows.Forms.Button();
            this.btnОтменаПоезда = new System.Windows.Forms.Button();
            this.cBОтправлениеЗадерживается = new System.Windows.Forms.CheckBox();
            this.cBПрибытиеЗадерживается = new System.Windows.Forms.CheckBox();
            this.cBПоездОтменен = new System.Windows.Forms.CheckBox();
            this.gBНастройкиПоезда = new System.Windows.Forms.GroupBox();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gB_НумерацияПоезда.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gB_Прибытие.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gBНастройкиПоезда.SuspendLayout();
            this.SuspendLayout();
            // 
            // gB_НумерацияПоезда
            // 
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_СХвоста);
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_СГоловы);
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_Отсутствует);
            this.gB_НумерацияПоезда.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_НумерацияПоезда.Location = new System.Drawing.Point(11, 93);
            this.gB_НумерацияПоезда.Name = "gB_НумерацияПоезда";
            this.gB_НумерацияПоезда.Size = new System.Drawing.Size(173, 115);
            this.gB_НумерацияПоезда.TabIndex = 0;
            this.gB_НумерацияПоезда.TabStop = false;
            this.gB_НумерацияПоезда.Text = "Нумерация поезда";
            // 
            // rB_Нумерация_СХвоста
            // 
            this.rB_Нумерация_СХвоста.AutoSize = true;
            this.rB_Нумерация_СХвоста.Location = new System.Drawing.Point(15, 69);
            this.rB_Нумерация_СХвоста.Name = "rB_Нумерация_СХвоста";
            this.rB_Нумерация_СХвоста.Size = new System.Drawing.Size(143, 25);
            this.rB_Нумерация_СХвоста.TabIndex = 3;
            this.rB_Нумерация_СХвоста.TabStop = true;
            this.rB_Нумерация_СХвоста.Text = "с хвоста состава";
            this.rB_Нумерация_СХвоста.UseVisualStyleBackColor = true;
            this.rB_Нумерация_СХвоста.CheckedChanged += new System.EventHandler(this.rB_Нумерация_CheckedChanged);
            // 
            // rB_Нумерация_СГоловы
            // 
            this.rB_Нумерация_СГоловы.AutoSize = true;
            this.rB_Нумерация_СГоловы.Location = new System.Drawing.Point(15, 46);
            this.rB_Нумерация_СГоловы.Name = "rB_Нумерация_СГоловы";
            this.rB_Нумерация_СГоловы.Size = new System.Drawing.Size(148, 25);
            this.rB_Нумерация_СГоловы.TabIndex = 2;
            this.rB_Нумерация_СГоловы.TabStop = true;
            this.rB_Нумерация_СГоловы.Text = "с головы состава";
            this.rB_Нумерация_СГоловы.UseVisualStyleBackColor = true;
            this.rB_Нумерация_СГоловы.CheckedChanged += new System.EventHandler(this.rB_Нумерация_CheckedChanged);
            // 
            // rB_Нумерация_Отсутствует
            // 
            this.rB_Нумерация_Отсутствует.AutoSize = true;
            this.rB_Нумерация_Отсутствует.Checked = true;
            this.rB_Нумерация_Отсутствует.Location = new System.Drawing.Point(15, 23);
            this.rB_Нумерация_Отсутствует.Name = "rB_Нумерация_Отсутствует";
            this.rB_Нумерация_Отсутствует.Size = new System.Drawing.Size(111, 25);
            this.rB_Нумерация_Отсутствует.TabIndex = 1;
            this.rB_Нумерация_Отсутствует.TabStop = true;
            this.rB_Нумерация_Отсутствует.Text = "отсутствует";
            this.rB_Нумерация_Отсутствует.UseVisualStyleBackColor = true;
            this.rB_Нумерация_Отсутствует.CheckedChanged += new System.EventHandler(this.rB_Нумерация_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lVШаблоны);
            this.groupBox2.Controls.Add(this.rTB_Сообщение);
            this.groupBox2.Controls.Add(this.btnВоспроизвестиВыбранныйШаблон);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(11, 406);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(631, 270);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Шаблоны оповещения";
            // 
            // lVШаблоны
            // 
            this.lVШаблоны.CheckBoxes = true;
            this.lVШаблоны.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lVШаблоны.FullRowSelect = true;
            this.lVШаблоны.GridLines = true;
            this.lVШаблоны.Location = new System.Drawing.Point(13, 29);
            this.lVШаблоны.Name = "lVШаблоны";
            this.lVШаблоны.Size = new System.Drawing.Size(608, 129);
            this.lVШаблоны.TabIndex = 1;
            this.lVШаблоны.UseCompatibleStateImageBehavior = false;
            this.lVШаблоны.View = System.Windows.Forms.View.Details;
            this.lVШаблоны.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lVШаблоны_ItemChecked);
            this.lVШаблоны.SelectedIndexChanged += new System.EventHandler(this.lVШаблоны_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Время";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Шаблон";
            this.columnHeader2.Width = 400;
            // 
            // rTB_Сообщение
            // 
            this.rTB_Сообщение.Location = new System.Drawing.Point(12, 164);
            this.rTB_Сообщение.Name = "rTB_Сообщение";
            this.rTB_Сообщение.Size = new System.Drawing.Size(429, 85);
            this.rTB_Сообщение.TabIndex = 0;
            this.rTB_Сообщение.Text = "";
            // 
            // btnВоспроизвестиВыбранныйШаблон
            // 
            this.btnВоспроизвестиВыбранныйШаблон.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnВоспроизвестиВыбранныйШаблон.Location = new System.Drawing.Point(447, 164);
            this.btnВоспроизвестиВыбранныйШаблон.Name = "btnВоспроизвестиВыбранныйШаблон";
            this.btnВоспроизвестиВыбранныйШаблон.Size = new System.Drawing.Size(174, 85);
            this.btnВоспроизвестиВыбранныйШаблон.TabIndex = 46;
            this.btnВоспроизвестиВыбранныйШаблон.Text = "ВОСПРОИЗВЕСТИ ВЫБРАННЫЙ ШАБЛОН";
            this.btnВоспроизвестиВыбранныйШаблон.UseVisualStyleBackColor = true;
            this.btnВоспроизвестиВыбранныйШаблон.Click += new System.EventHandler(this.btnВоспроизвестиВыбранныйШаблон_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер пути:";
            // 
            // cB_НомерПути
            // 
            this.cB_НомерПути.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cB_НомерПути.FormattingEnabled = true;
            this.cB_НомерПути.Location = new System.Drawing.Point(120, 58);
            this.cB_НомерПути.Name = "cB_НомерПути";
            this.cB_НомерПути.Size = new System.Drawing.Size(192, 29);
            this.cB_НомерПути.TabIndex = 5;
            this.cB_НомерПути.Text = "Неизвестно";
            this.cB_НомерПути.SelectedIndexChanged += new System.EventHandler(this.cB_НомерПути_SelectedIndexChanged);
            // 
            // btn_Подтвердить
            // 
            this.btn_Подтвердить.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Подтвердить.Location = new System.Drawing.Point(386, 790);
            this.btn_Подтвердить.Name = "btn_Подтвердить";
            this.btn_Подтвердить.Size = new System.Drawing.Size(122, 39);
            this.btn_Подтвердить.TabIndex = 6;
            this.btn_Подтвердить.Text = "Подтвердить";
            this.btn_Подтвердить.UseVisualStyleBackColor = true;
            this.btn_Подтвердить.Click += new System.EventHandler(this.btn_Подтвердить_Click);
            // 
            // gB_Прибытие
            // 
            this.gB_Прибытие.Controls.Add(this.cBОтправление);
            this.gB_Прибытие.Controls.Add(this.cBПрибытие);
            this.gB_Прибытие.Controls.Add(this.btn_ИзменитьВремяОтправления);
            this.gB_Прибытие.Controls.Add(this.dTP_ВремяОтправления);
            this.gB_Прибытие.Controls.Add(this.btn_ИзменитьВремяПрибытия);
            this.gB_Прибытие.Controls.Add(this.label5);
            this.gB_Прибытие.Controls.Add(this.dTP_Прибытие);
            this.gB_Прибытие.Controls.Add(this.label3);
            this.gB_Прибытие.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_Прибытие.Location = new System.Drawing.Point(190, 104);
            this.gB_Прибытие.Name = "gB_Прибытие";
            this.gB_Прибытие.Size = new System.Drawing.Size(450, 104);
            this.gB_Прибытие.TabIndex = 4;
            this.gB_Прибытие.TabStop = false;
            this.gB_Прибытие.Text = "Время движения";
            // 
            // cBОтправление
            // 
            this.cBОтправление.AutoSize = true;
            this.cBОтправление.Location = new System.Drawing.Point(388, 64);
            this.cBОтправление.Name = "cBОтправление";
            this.cBОтправление.Size = new System.Drawing.Size(57, 25);
            this.cBОтправление.TabIndex = 14;
            this.cBОтправление.Text = "Вкл.";
            this.cBОтправление.UseVisualStyleBackColor = true;
            this.cBОтправление.CheckedChanged += new System.EventHandler(this.cBОтправление_CheckedChanged);
            // 
            // cBПрибытие
            // 
            this.cBПрибытие.AutoSize = true;
            this.cBПрибытие.Location = new System.Drawing.Point(388, 25);
            this.cBПрибытие.Name = "cBПрибытие";
            this.cBПрибытие.Size = new System.Drawing.Size(57, 25);
            this.cBПрибытие.TabIndex = 13;
            this.cBПрибытие.Text = "Вкл.";
            this.cBПрибытие.UseVisualStyleBackColor = true;
            this.cBПрибытие.CheckedChanged += new System.EventHandler(this.cBПрибытие_CheckedChanged);
            // 
            // btn_ИзменитьВремяОтправления
            // 
            this.btn_ИзменитьВремяОтправления.Location = new System.Drawing.Point(241, 61);
            this.btn_ИзменитьВремяОтправления.Name = "btn_ИзменитьВремяОтправления";
            this.btn_ИзменитьВремяОтправления.Size = new System.Drawing.Size(130, 29);
            this.btn_ИзменитьВремяОтправления.TabIndex = 12;
            this.btn_ИзменитьВремяОтправления.Text = "Изменить";
            this.btn_ИзменитьВремяОтправления.UseVisualStyleBackColor = true;
            this.btn_ИзменитьВремяОтправления.Click += new System.EventHandler(this.btn_ИзменитьВремяОтправления_Click);
            // 
            // dTP_ВремяОтправления
            // 
            this.dTP_ВремяОтправления.CustomFormat = "HH:mm";
            this.dTP_ВремяОтправления.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_ВремяОтправления.Location = new System.Drawing.Point(150, 61);
            this.dTP_ВремяОтправления.Name = "dTP_ВремяОтправления";
            this.dTP_ВремяОтправления.ShowUpDown = true;
            this.dTP_ВремяОтправления.Size = new System.Drawing.Size(85, 29);
            this.dTP_ВремяОтправления.TabIndex = 11;
            // 
            // btn_ИзменитьВремяПрибытия
            // 
            this.btn_ИзменитьВремяПрибытия.Location = new System.Drawing.Point(241, 24);
            this.btn_ИзменитьВремяПрибытия.Name = "btn_ИзменитьВремяПрибытия";
            this.btn_ИзменитьВремяПрибытия.Size = new System.Drawing.Size(131, 29);
            this.btn_ИзменитьВремяПрибытия.TabIndex = 12;
            this.btn_ИзменитьВремяПрибытия.Text = "Изменить";
            this.btn_ИзменитьВремяПрибытия.UseVisualStyleBackColor = true;
            this.btn_ИзменитьВремяПрибытия.Click += new System.EventHandler(this.btn_ИзменитьВремяПрибытия_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(4, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 21);
            this.label5.TabIndex = 10;
            this.label5.Text = "Время отправл.:";
            // 
            // dTP_Прибытие
            // 
            this.dTP_Прибытие.CustomFormat = "HH:mm";
            this.dTP_Прибытие.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Прибытие.Location = new System.Drawing.Point(150, 24);
            this.dTP_Прибытие.Name = "dTP_Прибытие";
            this.dTP_Прибытие.ShowUpDown = true;
            this.dTP_Прибытие.Size = new System.Drawing.Size(85, 29);
            this.dTP_Прибытие.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(4, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "Время прибытия:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(514, 790);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 39);
            this.button1.TabIndex = 14;
            this.button1.Text = "Отмена";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox_displayTable
            // 
            this.comboBox_displayTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_displayTable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_displayTable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox_displayTable.FormattingEnabled = true;
            this.comboBox_displayTable.Location = new System.Drawing.Point(428, 60);
            this.comboBox_displayTable.MaxDropDownItems = 20;
            this.comboBox_displayTable.Name = "comboBox_displayTable";
            this.comboBox_displayTable.Size = new System.Drawing.Size(212, 29);
            this.comboBox_displayTable.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(315, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 21);
            this.label6.TabIndex = 17;
            this.label6.Text = "Список табло";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rB_СоВсемиОстановками);
            this.groupBox1.Controls.Add(this.btnРедактировать);
            this.groupBox1.Controls.Add(this.lB_ПоСтанциям);
            this.groupBox1.Controls.Add(this.rB_КромеСтанций);
            this.groupBox1.Controls.Add(this.rB_ПоСтанциям);
            this.groupBox1.Controls.Add(this.rB_ПоРасписанию);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(11, 213);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(629, 154);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Движение с остановками";
            // 
            // rB_СоВсемиОстановками
            // 
            this.rB_СоВсемиОстановками.AutoSize = true;
            this.rB_СоВсемиОстановками.Location = new System.Drawing.Point(13, 87);
            this.rB_СоВсемиОстановками.Margin = new System.Windows.Forms.Padding(2);
            this.rB_СоВсемиОстановками.Name = "rB_СоВсемиОстановками";
            this.rB_СоВсемиОстановками.Size = new System.Drawing.Size(197, 24);
            this.rB_СоВсемиОстановками.TabIndex = 18;
            this.rB_СоВсемиОстановками.TabStop = true;
            this.rB_СоВсемиОстановками.Text = "со всеми остановками";
            this.rB_СоВсемиОстановками.UseVisualStyleBackColor = true;
            this.rB_СоВсемиОстановками.CheckedChanged += new System.EventHandler(this.rB_ПоСтанциям_CheckedChanged);
            // 
            // btnРедактировать
            // 
            this.btnРедактировать.Enabled = false;
            this.btnРедактировать.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnРедактировать.Location = new System.Drawing.Point(11, 116);
            this.btnРедактировать.Margin = new System.Windows.Forms.Padding(2);
            this.btnРедактировать.Name = "btnРедактировать";
            this.btnРедактировать.Size = new System.Drawing.Size(228, 29);
            this.btnРедактировать.TabIndex = 13;
            this.btnРедактировать.Text = "РЕДАКТИРОВАТЬ";
            this.btnРедактировать.UseVisualStyleBackColor = true;
            this.btnРедактировать.Click += new System.EventHandler(this.button2_Click);
            // 
            // lB_ПоСтанциям
            // 
            this.lB_ПоСтанциям.Enabled = false;
            this.lB_ПоСтанциям.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lB_ПоСтанциям.FormattingEnabled = true;
            this.lB_ПоСтанциям.ItemHeight = 20;
            this.lB_ПоСтанциям.Location = new System.Drawing.Point(278, 22);
            this.lB_ПоСтанциям.Margin = new System.Windows.Forms.Padding(2);
            this.lB_ПоСтанциям.Name = "lB_ПоСтанциям";
            this.lB_ПоСтанциям.Size = new System.Drawing.Size(343, 124);
            this.lB_ПоСтанциям.Sorted = true;
            this.lB_ПоСтанциям.TabIndex = 3;
            // 
            // rB_КромеСтанций
            // 
            this.rB_КромеСтанций.AutoSize = true;
            this.rB_КромеСтанций.Location = new System.Drawing.Point(13, 65);
            this.rB_КромеСтанций.Margin = new System.Windows.Forms.Padding(2);
            this.rB_КромеСтанций.Name = "rB_КромеСтанций";
            this.rB_КромеСтанций.Size = new System.Drawing.Size(215, 24);
            this.rB_КромеСтанций.TabIndex = 2;
            this.rB_КромеСтанций.TabStop = true;
            this.rB_КромеСтанций.Text = "кроме станций из списка";
            this.rB_КромеСтанций.UseVisualStyleBackColor = true;
            this.rB_КромеСтанций.CheckedChanged += new System.EventHandler(this.rB_ПоСтанциям_CheckedChanged);
            // 
            // rB_ПоСтанциям
            // 
            this.rB_ПоСтанциям.AutoSize = true;
            this.rB_ПоСтанциям.Location = new System.Drawing.Point(13, 43);
            this.rB_ПоСтанциям.Margin = new System.Windows.Forms.Padding(2);
            this.rB_ПоСтанциям.Name = "rB_ПоСтанциям";
            this.rB_ПоСтанциям.Size = new System.Drawing.Size(198, 24);
            this.rB_ПоСтанциям.TabIndex = 1;
            this.rB_ПоСтанциям.TabStop = true;
            this.rB_ПоСтанциям.Text = "по станциям из списка";
            this.rB_ПоСтанциям.UseVisualStyleBackColor = true;
            this.rB_ПоСтанциям.CheckedChanged += new System.EventHandler(this.rB_ПоСтанциям_CheckedChanged);
            // 
            // rB_ПоРасписанию
            // 
            this.rB_ПоРасписанию.AutoSize = true;
            this.rB_ПоРасписанию.Checked = true;
            this.rB_ПоРасписанию.Location = new System.Drawing.Point(13, 21);
            this.rB_ПоРасписанию.Margin = new System.Windows.Forms.Padding(2);
            this.rB_ПоРасписанию.Name = "rB_ПоРасписанию";
            this.rB_ПоРасписанию.Size = new System.Drawing.Size(136, 24);
            this.rB_ПоРасписанию.TabIndex = 0;
            this.rB_ПоРасписанию.TabStop = true;
            this.rB_ПоРасписанию.Text = "не озвучивать";
            this.rB_ПоРасписанию.UseVisualStyleBackColor = true;
            this.rB_ПоРасписанию.CheckedChanged += new System.EventHandler(this.rB_ПоСтанциям_CheckedChanged);
            // 
            // cBКуда
            // 
            this.cBКуда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBКуда.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBКуда.FormattingEnabled = true;
            this.cBКуда.Location = new System.Drawing.Point(385, 19);
            this.cBКуда.Name = "cBКуда";
            this.cBКуда.Size = new System.Drawing.Size(255, 28);
            this.cBКуда.TabIndex = 43;
            // 
            // cBОткуда
            // 
            this.cBОткуда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBОткуда.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBОткуда.FormattingEnabled = true;
            this.cBОткуда.Location = new System.Drawing.Point(116, 19);
            this.cBОткуда.Name = "cBОткуда";
            this.cBОткуда.Size = new System.Drawing.Size(263, 28);
            this.cBОткуда.TabIndex = 42;
            // 
            // cBНомерПоезда
            // 
            this.cBНомерПоезда.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBНомерПоезда.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBНомерПоезда.FormattingEnabled = true;
            this.cBНомерПоезда.Location = new System.Drawing.Point(11, 19);
            this.cBНомерПоезда.Name = "cBНомерПоезда";
            this.cBНомерПоезда.Size = new System.Drawing.Size(99, 28);
            this.cBНомерПоезда.TabIndex = 44;
            // 
            // btnПовторения
            // 
            this.btnПовторения.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnПовторения.Location = new System.Drawing.Point(177, 371);
            this.btnПовторения.Margin = new System.Windows.Forms.Padding(2);
            this.btnПовторения.Name = "btnПовторения";
            this.btnПовторения.Size = new System.Drawing.Size(135, 29);
            this.btnПовторения.TabIndex = 19;
            this.btnПовторения.Text = "1 ПОВТОР";
            this.btnПовторения.UseVisualStyleBackColor = true;
            this.btnПовторения.Click += new System.EventHandler(this.btnПовторения_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(20, 375);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 21);
            this.label2.TabIndex = 45;
            this.label2.Text = "Кол-во повторений:";
            // 
            // cBОтменен
            // 
            this.cBОтменен.AutoSize = true;
            this.cBОтменен.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBОтменен.Location = new System.Drawing.Point(23, 799);
            this.cBОтменен.Name = "cBОтменен";
            this.cBОтменен.Size = new System.Drawing.Size(257, 24);
            this.cBОтменен.TabIndex = 15;
            this.cBОтменен.Text = "Отменен (без объявления)";
            this.cBОтменен.UseVisualStyleBackColor = true;
            this.cBОтменен.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.NavajoWhite;
            this.groupBox3.Controls.Add(this.btnЗадержкаОтправления);
            this.groupBox3.Controls.Add(this.btnЗадержкаПрибытия);
            this.groupBox3.Controls.Add(this.btnОтменаПоезда);
            this.groupBox3.Controls.Add(this.cBОтправлениеЗадерживается);
            this.groupBox3.Controls.Add(this.cBПрибытиеЗадерживается);
            this.groupBox3.Controls.Add(this.cBПоездОтменен);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(14, 683);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(629, 101);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Нештатные ситуации";
            // 
            // btnЗадержкаОтправления
            // 
            this.btnЗадержкаОтправления.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnЗадержкаОтправления.Location = new System.Drawing.Point(479, 70);
            this.btnЗадержкаОтправления.Name = "btnЗадержкаОтправления";
            this.btnЗадержкаОтправления.Size = new System.Drawing.Size(144, 24);
            this.btnЗадержкаОтправления.TabIndex = 51;
            this.btnЗадержкаОтправления.Text = "ВОСПРОИЗВЕСТИ";
            this.btnЗадержкаОтправления.UseVisualStyleBackColor = true;
            this.btnЗадержкаОтправления.Click += new System.EventHandler(this.btnОтменаПоезда_Click);
            // 
            // btnЗадержкаПрибытия
            // 
            this.btnЗадержкаПрибытия.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnЗадержкаПрибытия.Location = new System.Drawing.Point(479, 43);
            this.btnЗадержкаПрибытия.Name = "btnЗадержкаПрибытия";
            this.btnЗадержкаПрибытия.Size = new System.Drawing.Size(144, 24);
            this.btnЗадержкаПрибытия.TabIndex = 50;
            this.btnЗадержкаПрибытия.Text = "ВОСПРОИЗВЕСТИ";
            this.btnЗадержкаПрибытия.UseVisualStyleBackColor = true;
            this.btnЗадержкаПрибытия.Click += new System.EventHandler(this.btnОтменаПоезда_Click);
            // 
            // btnОтменаПоезда
            // 
            this.btnОтменаПоезда.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnОтменаПоезда.Location = new System.Drawing.Point(479, 16);
            this.btnОтменаПоезда.Name = "btnОтменаПоезда";
            this.btnОтменаПоезда.Size = new System.Drawing.Size(144, 24);
            this.btnОтменаПоезда.TabIndex = 49;
            this.btnОтменаПоезда.Text = "ВОСПРОИЗВЕСТИ";
            this.btnОтменаПоезда.UseVisualStyleBackColor = true;
            this.btnОтменаПоезда.Click += new System.EventHandler(this.btnОтменаПоезда_Click);
            // 
            // cBОтправлениеЗадерживается
            // 
            this.cBОтправлениеЗадерживается.AutoSize = true;
            this.cBОтправлениеЗадерживается.Location = new System.Drawing.Point(11, 70);
            this.cBОтправлениеЗадерживается.Name = "cBОтправлениеЗадерживается";
            this.cBОтправлениеЗадерживается.Size = new System.Drawing.Size(432, 24);
            this.cBОтправлениеЗадерживается.TabIndex = 2;
            this.cBОтправлениеЗадерживается.Text = "Сообщение \"Отправление поезда задерживается ...\"";
            this.cBОтправлениеЗадерживается.UseVisualStyleBackColor = true;
            this.cBОтправлениеЗадерживается.CheckedChanged += new System.EventHandler(this.cBПоездОтменен_CheckedChanged);
            // 
            // cBПрибытиеЗадерживается
            // 
            this.cBПрибытиеЗадерживается.AutoSize = true;
            this.cBПрибытиеЗадерживается.Location = new System.Drawing.Point(11, 47);
            this.cBПрибытиеЗадерживается.Name = "cBПрибытиеЗадерживается";
            this.cBПрибытиеЗадерживается.Size = new System.Drawing.Size(406, 24);
            this.cBПрибытиеЗадерживается.TabIndex = 1;
            this.cBПрибытиеЗадерживается.Text = "Сообщение \"Прибытие поезда задерживается ...\"";
            this.cBПрибытиеЗадерживается.UseVisualStyleBackColor = true;
            this.cBПрибытиеЗадерживается.CheckedChanged += new System.EventHandler(this.cBПоездОтменен_CheckedChanged);
            // 
            // cBПоездОтменен
            // 
            this.cBПоездОтменен.AutoSize = true;
            this.cBПоездОтменен.Location = new System.Drawing.Point(11, 25);
            this.cBПоездОтменен.Name = "cBПоездОтменен";
            this.cBПоездОтменен.Size = new System.Drawing.Size(265, 24);
            this.cBПоездОтменен.TabIndex = 0;
            this.cBПоездОтменен.Text = "Сообщение \"Поезд отменен ...\"";
            this.cBПоездОтменен.UseVisualStyleBackColor = true;
            this.cBПоездОтменен.CheckedChanged += new System.EventHandler(this.cBПоездОтменен_CheckedChanged);
            // 
            // gBНастройкиПоезда
            // 
            this.gBНастройкиПоезда.Controls.Add(this.cBНомерПоезда);
            this.gBНастройкиПоезда.Controls.Add(this.gB_НумерацияПоезда);
            this.gBНастройкиПоезда.Controls.Add(this.groupBox2);
            this.gBНастройкиПоезда.Controls.Add(this.label1);
            this.gBНастройкиПоезда.Controls.Add(this.label2);
            this.gBНастройкиПоезда.Controls.Add(this.cB_НомерПути);
            this.gBНастройкиПоезда.Controls.Add(this.btnПовторения);
            this.gBНастройкиПоезда.Controls.Add(this.gB_Прибытие);
            this.gBНастройкиПоезда.Controls.Add(this.comboBox_displayTable);
            this.gBНастройкиПоезда.Controls.Add(this.cBКуда);
            this.gBНастройкиПоезда.Controls.Add(this.label6);
            this.gBНастройкиПоезда.Controls.Add(this.cBОткуда);
            this.gBНастройкиПоезда.Controls.Add(this.groupBox1);
            this.gBНастройкиПоезда.Location = new System.Drawing.Point(1, 1);
            this.gBНастройкиПоезда.Name = "gBНастройкиПоезда";
            this.gBНастройкиПоезда.Size = new System.Drawing.Size(652, 683);
            this.gBНастройкиПоезда.TabIndex = 48;
            this.gBНастройкиПоезда.TabStop = false;
            this.gBНастройкиПоезда.Text = "Настроки поезда";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Языки";
            this.columnHeader3.Width = 120;
            // 
            // КарточкаДвиженияПоезда
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 835);
            this.Controls.Add(this.gBНастройкиПоезда);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cBОтменен);
            this.Controls.Add(this.btn_Подтвердить);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "КарточкаДвиженияПоезда";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Карточка звукового сообщения";
            this.Load += new System.EventHandler(this.КарточкаДвиженияПоезда_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.КарточкаДвиженияПоезда_KeyDown);
            this.gB_НумерацияПоезда.ResumeLayout(false);
            this.gB_НумерацияПоезда.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gB_Прибытие.ResumeLayout(false);
            this.gB_Прибытие.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gBНастройкиПоезда.ResumeLayout(false);
            this.gBНастройкиПоезда.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gB_НумерацияПоезда;
        private System.Windows.Forms.RadioButton rB_Нумерация_СХвоста;
        private System.Windows.Forms.RadioButton rB_Нумерация_СГоловы;
        private System.Windows.Forms.RadioButton rB_Нумерация_Отсутствует;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rTB_Сообщение;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cB_НомерПути;
        private System.Windows.Forms.Button btn_Подтвердить;
        private System.Windows.Forms.GroupBox gB_Прибытие;
        private System.Windows.Forms.Button btn_ИзменитьВремяПрибытия;
        private System.Windows.Forms.DateTimePicker dTP_Прибытие;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_ИзменитьВремяОтправления;
        private System.Windows.Forms.DateTimePicker dTP_ВремяОтправления;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox_displayTable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnРедактировать;
        private System.Windows.Forms.ListBox lB_ПоСтанциям;
        private System.Windows.Forms.RadioButton rB_КромеСтанций;
        private System.Windows.Forms.RadioButton rB_ПоСтанциям;
        private System.Windows.Forms.RadioButton rB_ПоРасписанию;
        private System.Windows.Forms.RadioButton rB_СоВсемиОстановками;
        private System.Windows.Forms.ComboBox cBКуда;
        private System.Windows.Forms.ComboBox cBОткуда;
        private System.Windows.Forms.ComboBox cBНомерПоезда;
        private System.Windows.Forms.CheckBox cBОтправление;
        private System.Windows.Forms.CheckBox cBПрибытие;
        private System.Windows.Forms.Button btnПовторения;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lVШаблоны;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox cBОтменен;
        private System.Windows.Forms.Button btnВоспроизвестиВыбранныйШаблон;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox gBНастройкиПоезда;
        private System.Windows.Forms.Button btnЗадержкаОтправления;
        private System.Windows.Forms.Button btnЗадержкаПрибытия;
        private System.Windows.Forms.Button btnОтменаПоезда;
        private System.Windows.Forms.CheckBox cBОтправлениеЗадерживается;
        private System.Windows.Forms.CheckBox cBПрибытиеЗадерживается;
        private System.Windows.Forms.CheckBox cBПоездОтменен;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}