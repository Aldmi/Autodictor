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
            this.rTB_Сообщение = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cB_НомерПути = new System.Windows.Forms.ComboBox();
            this.btn_Подтвердить = new System.Windows.Forms.Button();
            this.gB_Прибытие = new System.Windows.Forms.GroupBox();
            this.btn_ИзменитьВремяПрибытия = new System.Windows.Forms.Button();
            this.dTP_Прибытие = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dTP_Время = new System.Windows.Forms.DateTimePicker();
            this.btn_ЗадатьВремя = new System.Windows.Forms.Button();
            this.gB_Стоянка = new System.Windows.Forms.GroupBox();
            this.tB_ВремяСтоянки = new System.Windows.Forms.TextBox();
            this.btn_ИзменитьВремяСтоянки = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.gB_Отправление = new System.Windows.Forms.GroupBox();
            this.btn_ИзменитьВремяОтправления = new System.Windows.Forms.Button();
            this.dTP_ВремяОтправления = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cB_ПрименитьКоВсем = new System.Windows.Forms.CheckBox();
            this.comboBox_displayTable = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.gB_НумерацияПоезда.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gB_Прибытие.SuspendLayout();
            this.gB_Стоянка.SuspendLayout();
            this.gB_Отправление.SuspendLayout();
            this.SuspendLayout();
            // 
            // gB_НумерацияПоезда
            // 
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_СХвоста);
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_СГоловы);
            this.gB_НумерацияПоезда.Controls.Add(this.rB_Нумерация_Отсутствует);
            this.gB_НумерацияПоезда.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_НумерацияПоезда.Location = new System.Drawing.Point(4, 332);
            this.gB_НумерацияПоезда.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_НумерацияПоезда.Name = "gB_НумерацияПоезда";
            this.gB_НумерацияПоезда.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_НумерацияПоезда.Size = new System.Drawing.Size(218, 127);
            this.gB_НумерацияПоезда.TabIndex = 0;
            this.gB_НумерацияПоезда.TabStop = false;
            this.gB_НумерацияПоезда.Text = "Нумерация поезда";
            // 
            // rB_Нумерация_СХвоста
            // 
            this.rB_Нумерация_СХвоста.AutoSize = true;
            this.rB_Нумерация_СХвоста.Location = new System.Drawing.Point(20, 85);
            this.rB_Нумерация_СХвоста.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rB_Нумерация_СХвоста.Name = "rB_Нумерация_СХвоста";
            this.rB_Нумерация_СХвоста.Size = new System.Drawing.Size(180, 32);
            this.rB_Нумерация_СХвоста.TabIndex = 3;
            this.rB_Нумерация_СХвоста.TabStop = true;
            this.rB_Нумерация_СХвоста.Text = "с хвоста состава";
            this.rB_Нумерация_СХвоста.UseVisualStyleBackColor = true;
            this.rB_Нумерация_СХвоста.CheckedChanged += new System.EventHandler(this.rB_Нумерация_CheckedChanged);
            // 
            // rB_Нумерация_СГоловы
            // 
            this.rB_Нумерация_СГоловы.AutoSize = true;
            this.rB_Нумерация_СГоловы.Location = new System.Drawing.Point(20, 57);
            this.rB_Нумерация_СГоловы.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rB_Нумерация_СГоловы.Name = "rB_Нумерация_СГоловы";
            this.rB_Нумерация_СГоловы.Size = new System.Drawing.Size(189, 32);
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
            this.rB_Нумерация_Отсутствует.Location = new System.Drawing.Point(20, 28);
            this.rB_Нумерация_Отсутствует.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rB_Нумерация_Отсутствует.Name = "rB_Нумерация_Отсутствует";
            this.rB_Нумерация_Отсутствует.Size = new System.Drawing.Size(136, 32);
            this.rB_Нумерация_Отсутствует.TabIndex = 1;
            this.rB_Нумерация_Отсутствует.TabStop = true;
            this.rB_Нумерация_Отсутствует.Text = "отсутствует";
            this.rB_Нумерация_Отсутствует.UseVisualStyleBackColor = true;
            this.rB_Нумерация_Отсутствует.CheckedChanged += new System.EventHandler(this.rB_Нумерация_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rTB_Сообщение);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(4, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(528, 169);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сообщение";
            // 
            // rTB_Сообщение
            // 
            this.rTB_Сообщение.Location = new System.Drawing.Point(7, 34);
            this.rTB_Сообщение.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rTB_Сообщение.Name = "rTB_Сообщение";
            this.rTB_Сообщение.Size = new System.Drawing.Size(512, 126);
            this.rTB_Сообщение.TabIndex = 0;
            this.rTB_Сообщение.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(5, 238);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер пути:";
            // 
            // cB_НомерПути
            // 
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
            this.cB_НомерПути.Location = new System.Drawing.Point(144, 239);
            this.cB_НомерПути.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cB_НомерПути.Name = "cB_НомерПути";
            this.cB_НомерПути.Size = new System.Drawing.Size(255, 36);
            this.cB_НомерПути.TabIndex = 5;
            this.cB_НомерПути.Text = "Неизвестно";
            this.cB_НомерПути.SelectedIndexChanged += new System.EventHandler(this.cB_НомерПути_SelectedIndexChanged);
            // 
            // btn_Подтвердить
            // 
            this.btn_Подтвердить.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Подтвердить.Location = new System.Drawing.Point(4, 706);
            this.btn_Подтвердить.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Подтвердить.Name = "btn_Подтвердить";
            this.btn_Подтвердить.Size = new System.Drawing.Size(251, 47);
            this.btn_Подтвердить.TabIndex = 6;
            this.btn_Подтвердить.Text = "Подтвердить";
            this.btn_Подтвердить.UseVisualStyleBackColor = true;
            this.btn_Подтвердить.Click += new System.EventHandler(this.btn_Подтвердить_Click);
            // 
            // gB_Прибытие
            // 
            this.gB_Прибытие.Controls.Add(this.btn_ИзменитьВремяПрибытия);
            this.gB_Прибытие.Controls.Add(this.dTP_Прибытие);
            this.gB_Прибытие.Controls.Add(this.label3);
            this.gB_Прибытие.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_Прибытие.Location = new System.Drawing.Point(4, 455);
            this.gB_Прибытие.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_Прибытие.Name = "gB_Прибытие";
            this.gB_Прибытие.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_Прибытие.Size = new System.Drawing.Size(520, 76);
            this.gB_Прибытие.TabIndex = 4;
            this.gB_Прибытие.TabStop = false;
            this.gB_Прибытие.Text = "Прибытие";
            // 
            // btn_ИзменитьВремяПрибытия
            // 
            this.btn_ИзменитьВремяПрибытия.Location = new System.Drawing.Point(321, 30);
            this.btn_ИзменитьВремяПрибытия.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ИзменитьВремяПрибытия.Name = "btn_ИзменитьВремяПрибытия";
            this.btn_ИзменитьВремяПрибытия.Size = new System.Drawing.Size(199, 36);
            this.btn_ИзменитьВремяПрибытия.TabIndex = 12;
            this.btn_ИзменитьВремяПрибытия.Text = "Изменить";
            this.btn_ИзменитьВремяПрибытия.UseVisualStyleBackColor = true;
            this.btn_ИзменитьВремяПрибытия.Click += new System.EventHandler(this.btn_ИзменитьВремяПрибытия_Click);
            // 
            // dTP_Прибытие
            // 
            this.dTP_Прибытие.CustomFormat = "HH:mm";
            this.dTP_Прибытие.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_Прибытие.Location = new System.Drawing.Point(200, 30);
            this.dTP_Прибытие.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dTP_Прибытие.Name = "dTP_Прибытие";
            this.dTP_Прибытие.ShowUpDown = true;
            this.dTP_Прибытие.Size = new System.Drawing.Size(112, 34);
            this.dTP_Прибытие.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(5, 33);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 28);
            this.label3.TabIndex = 10;
            this.label3.Text = "Время прибытия:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(5, 194);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 28);
            this.label2.TabIndex = 7;
            this.label2.Text = "Время сообщения:";
            // 
            // dTP_Время
            // 
            this.dTP_Время.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_Время.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dTP_Время.Location = new System.Drawing.Point(204, 191);
            this.dTP_Время.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dTP_Время.Name = "dTP_Время";
            this.dTP_Время.ShowUpDown = true;
            this.dTP_Время.Size = new System.Drawing.Size(112, 34);
            this.dTP_Время.TabIndex = 8;
            // 
            // btn_ЗадатьВремя
            // 
            this.btn_ЗадатьВремя.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ЗадатьВремя.Location = new System.Drawing.Point(325, 190);
            this.btn_ЗадатьВремя.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ЗадатьВремя.Name = "btn_ЗадатьВремя";
            this.btn_ЗадатьВремя.Size = new System.Drawing.Size(195, 37);
            this.btn_ЗадатьВремя.TabIndex = 9;
            this.btn_ЗадатьВремя.Text = "Изменить";
            this.btn_ЗадатьВремя.UseVisualStyleBackColor = true;
            this.btn_ЗадатьВремя.Click += new System.EventHandler(this.btn_ЗадатьВремя_Click);
            // 
            // gB_Стоянка
            // 
            this.gB_Стоянка.Controls.Add(this.tB_ВремяСтоянки);
            this.gB_Стоянка.Controls.Add(this.btn_ИзменитьВремяСтоянки);
            this.gB_Стоянка.Controls.Add(this.label4);
            this.gB_Стоянка.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_Стоянка.Location = new System.Drawing.Point(4, 539);
            this.gB_Стоянка.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_Стоянка.Name = "gB_Стоянка";
            this.gB_Стоянка.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_Стоянка.Size = new System.Drawing.Size(520, 76);
            this.gB_Стоянка.TabIndex = 13;
            this.gB_Стоянка.TabStop = false;
            this.gB_Стоянка.Text = "Стоянка";
            // 
            // tB_ВремяСтоянки
            // 
            this.tB_ВремяСтоянки.Location = new System.Drawing.Point(200, 30);
            this.tB_ВремяСтоянки.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tB_ВремяСтоянки.Name = "tB_ВремяСтоянки";
            this.tB_ВремяСтоянки.Size = new System.Drawing.Size(112, 34);
            this.tB_ВремяСтоянки.TabIndex = 13;
            this.tB_ВремяСтоянки.Text = "5";
            // 
            // btn_ИзменитьВремяСтоянки
            // 
            this.btn_ИзменитьВремяСтоянки.Location = new System.Drawing.Point(321, 30);
            this.btn_ИзменитьВремяСтоянки.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ИзменитьВремяСтоянки.Name = "btn_ИзменитьВремяСтоянки";
            this.btn_ИзменитьВремяСтоянки.Size = new System.Drawing.Size(199, 36);
            this.btn_ИзменитьВремяСтоянки.TabIndex = 12;
            this.btn_ИзменитьВремяСтоянки.Text = "Изменить";
            this.btn_ИзменитьВремяСтоянки.UseVisualStyleBackColor = true;
            this.btn_ИзменитьВремяСтоянки.Click += new System.EventHandler(this.btn_ИзменитьВремяСтоянки_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(5, 33);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 28);
            this.label4.TabIndex = 10;
            this.label4.Text = "Время стоянки:";
            // 
            // gB_Отправление
            // 
            this.gB_Отправление.Controls.Add(this.btn_ИзменитьВремяОтправления);
            this.gB_Отправление.Controls.Add(this.dTP_ВремяОтправления);
            this.gB_Отправление.Controls.Add(this.label5);
            this.gB_Отправление.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_Отправление.Location = new System.Drawing.Point(4, 623);
            this.gB_Отправление.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_Отправление.Name = "gB_Отправление";
            this.gB_Отправление.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gB_Отправление.Size = new System.Drawing.Size(520, 76);
            this.gB_Отправление.TabIndex = 13;
            this.gB_Отправление.TabStop = false;
            this.gB_Отправление.Text = "Отправление";
            // 
            // btn_ИзменитьВремяОтправления
            // 
            this.btn_ИзменитьВремяОтправления.Location = new System.Drawing.Point(321, 30);
            this.btn_ИзменитьВремяОтправления.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ИзменитьВремяОтправления.Name = "btn_ИзменитьВремяОтправления";
            this.btn_ИзменитьВремяОтправления.Size = new System.Drawing.Size(199, 36);
            this.btn_ИзменитьВремяОтправления.TabIndex = 12;
            this.btn_ИзменитьВремяОтправления.Text = "Изменить";
            this.btn_ИзменитьВремяОтправления.UseVisualStyleBackColor = true;
            this.btn_ИзменитьВремяОтправления.Click += new System.EventHandler(this.btn_ИзменитьВремяОтправления_Click);
            // 
            // dTP_ВремяОтправления
            // 
            this.dTP_ВремяОтправления.CustomFormat = "HH:mm";
            this.dTP_ВремяОтправления.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_ВремяОтправления.Location = new System.Drawing.Point(200, 30);
            this.dTP_ВремяОтправления.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dTP_ВремяОтправления.Name = "dTP_ВремяОтправления";
            this.dTP_ВремяОтправления.ShowUpDown = true;
            this.dTP_ВремяОтправления.Size = new System.Drawing.Size(112, 34);
            this.dTP_ВремяОтправления.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(5, 33);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 28);
            this.label5.TabIndex = 10;
            this.label5.Text = "Время отправл.:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(263, 706);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(257, 47);
            this.button1.TabIndex = 14;
            this.button1.Text = "Отмена";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cB_ПрименитьКоВсем
            // 
            this.cB_ПрименитьКоВсем.AutoSize = true;
            this.cB_ПрименитьКоВсем.Checked = true;
            this.cB_ПрименитьКоВсем.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cB_ПрименитьКоВсем.Location = new System.Drawing.Point(408, 241);
            this.cB_ПрименитьКоВсем.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cB_ПрименитьКоВсем.Name = "cB_ПрименитьКоВсем";
            this.cB_ПрименитьКоВсем.Size = new System.Drawing.Size(104, 21);
            this.cB_ПрименитьКоВсем.TabIndex = 15;
            this.cB_ПрименитьКоВсем.Text = "Все сообщ.";
            this.cB_ПрименитьКоВсем.UseVisualStyleBackColor = true;
            // 
            // comboBox_displayTable
            // 
            this.comboBox_displayTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_displayTable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_displayTable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox_displayTable.FormattingEnabled = true;
            this.comboBox_displayTable.Location = new System.Drawing.Point(144, 292);
            this.comboBox_displayTable.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_displayTable.MaxDropDownItems = 20;
            this.comboBox_displayTable.Name = "comboBox_displayTable";
            this.comboBox_displayTable.Size = new System.Drawing.Size(255, 36);
            this.comboBox_displayTable.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(5, 292);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 28);
            this.label6.TabIndex = 17;
            this.label6.Text = "Список табло";
            // 
            // КарточкаДвиженияПоезда
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 769);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox_displayTable);
            this.Controls.Add(this.cB_ПрименитьКоВсем);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gB_Отправление);
            this.Controls.Add(this.gB_Стоянка);
            this.Controls.Add(this.btn_ЗадатьВремя);
            this.Controls.Add(this.dTP_Время);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gB_Прибытие);
            this.Controls.Add(this.btn_Подтвердить);
            this.Controls.Add(this.cB_НомерПути);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gB_НумерацияПоезда);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "КарточкаДвиженияПоезда";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Карточка звукового сообщения";
            this.gB_НумерацияПоезда.ResumeLayout(false);
            this.gB_НумерацияПоезда.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gB_Прибытие.ResumeLayout(false);
            this.gB_Прибытие.PerformLayout();
            this.gB_Стоянка.ResumeLayout(false);
            this.gB_Стоянка.PerformLayout();
            this.gB_Отправление.ResumeLayout(false);
            this.gB_Отправление.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dTP_Время;
        private System.Windows.Forms.Button btn_ЗадатьВремя;
        private System.Windows.Forms.Button btn_ИзменитьВремяПрибытия;
        private System.Windows.Forms.DateTimePicker dTP_Прибытие;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gB_Стоянка;
        private System.Windows.Forms.TextBox tB_ВремяСтоянки;
        private System.Windows.Forms.Button btn_ИзменитьВремяСтоянки;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gB_Отправление;
        private System.Windows.Forms.Button btn_ИзменитьВремяОтправления;
        private System.Windows.Forms.DateTimePicker dTP_ВремяОтправления;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cB_ПрименитьКоВсем;
        private System.Windows.Forms.ComboBox comboBox_displayTable;
        private System.Windows.Forms.Label label6;
    }
}