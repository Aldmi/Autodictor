namespace MainExample
{
    partial class MainWindowForm
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
            this.lblTime = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDayOfWeek = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnБлокировка = new System.Windows.Forms.Button();
            this.lblСостояние = new System.Windows.Forms.Label();
            this.pnСостояние = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cB_ВоспроизведениеДвиженияПоездов = new System.Windows.Forms.CheckBox();
            this.btnОбновитьСписок = new System.Windows.Forms.Button();
            this.cBАвтоматическаяГенерация = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Player_Label = new System.Windows.Forms.Label();
            this.btnВоспроизвести = new System.Windows.Forms.Button();
            this.tBРегуляторГромкости = new System.Windows.Forms.TrackBar();
            this.pnСостояниеCIS = new System.Windows.Forms.Panel();
            this.lblСостояниеCIS = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.pnСостояние.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBРегуляторГромкости)).BeginInit();
            this.pnСостояниеCIS.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblTime.Location = new System.Drawing.Point(8, 60);
            this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(263, 46);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "0:01:22";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.columnHeader5,
            this.columnHeader4});
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(5, 197);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1561, 600);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Время";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Длительность";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 131;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Путь";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Описание";
            this.columnHeader4.Width = 1600;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.lblDayOfWeek);
            this.groupBox1.Controls.Add(this.lblDate);
            this.groupBox1.Controls.Add(this.lblTime);
            this.groupBox1.Location = new System.Drawing.Point(5, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(279, 151);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Текущие дата и время";
            // 
            // lblDayOfWeek
            // 
            this.lblDayOfWeek.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDayOfWeek.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblDayOfWeek.Location = new System.Drawing.Point(8, 106);
            this.lblDayOfWeek.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayOfWeek.Name = "lblDayOfWeek";
            this.lblDayOfWeek.Size = new System.Drawing.Size(263, 38);
            this.lblDayOfWeek.TabIndex = 3;
            this.lblDayOfWeek.Text = "среда";
            this.lblDayOfWeek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDate.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblDate.Location = new System.Drawing.Point(8, 16);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(263, 38);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "11.02.2014";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnБлокировка
            // 
            this.btnБлокировка.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnБлокировка.Location = new System.Drawing.Point(292, 23);
            this.btnБлокировка.Margin = new System.Windows.Forms.Padding(4);
            this.btnБлокировка.Name = "btnБлокировка";
            this.btnБлокировка.Size = new System.Drawing.Size(279, 70);
            this.btnБлокировка.TabIndex = 10;
            this.btnБлокировка.Text = "ОТКЛЮЧИТЬ";
            this.btnБлокировка.UseVisualStyleBackColor = true;
            this.btnБлокировка.Click += new System.EventHandler(this.btnБлокировка_Click);
            // 
            // lblСостояние
            // 
            this.lblСостояние.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblСостояние.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblСостояние.Location = new System.Drawing.Point(4, 1);
            this.lblСостояние.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblСостояние.Name = "lblСостояние";
            this.lblСостояние.Size = new System.Drawing.Size(271, 31);
            this.lblСостояние.TabIndex = 11;
            this.lblСостояние.Text = "РАБОТА";
            this.lblСостояние.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnСостояние
            // 
            this.pnСостояние.Controls.Add(this.lblСостояние);
            this.pnСостояние.Location = new System.Drawing.Point(292, 96);
            this.pnСостояние.Margin = new System.Windows.Forms.Padding(4);
            this.pnСостояние.Name = "pnСостояние";
            this.pnСостояние.Size = new System.Drawing.Size(279, 34);
            this.pnСостояние.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cB_ВоспроизведениеДвиженияПоездов);
            this.groupBox2.Controls.Add(this.btnОбновитьСписок);
            this.groupBox2.Controls.Add(this.cBАвтоматическаяГенерация);
            this.groupBox2.Location = new System.Drawing.Point(1020, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(279, 151);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Генерация списка";
            // 
            // cB_ВоспроизведениеДвиженияПоездов
            // 
            this.cB_ВоспроизведениеДвиженияПоездов.AutoSize = true;
            this.cB_ВоспроизведениеДвиженияПоездов.Checked = true;
            this.cB_ВоспроизведениеДвиженияПоездов.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cB_ВоспроизведениеДвиженияПоездов.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cB_ВоспроизведениеДвиженияПоездов.Location = new System.Drawing.Point(11, 114);
            this.cB_ВоспроизведениеДвиженияПоездов.Margin = new System.Windows.Forms.Padding(4);
            this.cB_ВоспроизведениеДвиженияПоездов.Name = "cB_ВоспроизведениеДвиженияПоездов";
            this.cB_ВоспроизведениеДвиженияПоездов.Size = new System.Drawing.Size(236, 29);
            this.cB_ВоспроизведениеДвиженияПоездов.TabIndex = 2;
            this.cB_ВоспроизведениеДвиженияПоездов.Text = "Воспр. движ. поездов";
            this.cB_ВоспроизведениеДвиженияПоездов.UseVisualStyleBackColor = true;
            this.cB_ВоспроизведениеДвиженияПоездов.CheckedChanged += new System.EventHandler(this.cB_ВоспроизведениеДвиженияПоездов_CheckedChanged);
            // 
            // btnОбновитьСписок
            // 
            this.btnОбновитьСписок.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnОбновитьСписок.Location = new System.Drawing.Point(11, 60);
            this.btnОбновитьСписок.Margin = new System.Windows.Forms.Padding(4);
            this.btnОбновитьСписок.Name = "btnОбновитьСписок";
            this.btnОбновитьСписок.Size = new System.Drawing.Size(260, 44);
            this.btnОбновитьСписок.TabIndex = 1;
            this.btnОбновитьСписок.Text = "Обновить список";
            this.btnОбновитьСписок.UseVisualStyleBackColor = true;
            this.btnОбновитьСписок.Click += new System.EventHandler(this.btnОбновитьСписок_Click);
            // 
            // cBАвтоматическаяГенерация
            // 
            this.cBАвтоматическаяГенерация.AutoSize = true;
            this.cBАвтоматическаяГенерация.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cBАвтоматическаяГенерация.Location = new System.Drawing.Point(11, 23);
            this.cBАвтоматическаяГенерация.Margin = new System.Windows.Forms.Padding(4);
            this.cBАвтоматическаяГенерация.Name = "cBАвтоматическаяГенерация";
            this.cBАвтоматическаяГенерация.Size = new System.Drawing.Size(175, 29);
            this.cBАвтоматическаяГенерация.TabIndex = 0;
            this.cBАвтоматическаяГенерация.Text = "Авт. генерация";
            this.cBАвтоматическаяГенерация.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.Player_Label);
            this.groupBox3.Controls.Add(this.btnВоспроизвести);
            this.groupBox3.Controls.Add(this.tBРегуляторГромкости);
            this.groupBox3.Location = new System.Drawing.Point(579, 15);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(433, 151);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Воспроизведение";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(8, 114);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 25);
            this.label1.TabIndex = 15;
            this.label1.Text = "Время / Длительность:";
            // 
            // Player_Label
            // 
            this.Player_Label.AutoSize = true;
            this.Player_Label.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Player_Label.Location = new System.Drawing.Point(277, 114);
            this.Player_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Player_Label.Name = "Player_Label";
            this.Player_Label.Size = new System.Drawing.Size(130, 28);
            this.Player_Label.TabIndex = 24;
            this.Player_Label.Text = "00:00 / 00:00";
            // 
            // btnВоспроизвести
            // 
            this.btnВоспроизвести.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnВоспроизвести.Location = new System.Drawing.Point(11, 68);
            this.btnВоспроизвести.Margin = new System.Windows.Forms.Padding(4);
            this.btnВоспроизвести.Name = "btnВоспроизвести";
            this.btnВоспроизвести.Size = new System.Drawing.Size(407, 37);
            this.btnВоспроизвести.TabIndex = 1;
            this.btnВоспроизвести.Text = "Воспроизвести выбранную запись";
            this.btnВоспроизвести.UseVisualStyleBackColor = true;
            this.btnВоспроизвести.Click += new System.EventHandler(this.btnВоспроизвести_Click);
            // 
            // tBРегуляторГромкости
            // 
            this.tBРегуляторГромкости.Location = new System.Drawing.Point(11, 23);
            this.tBРегуляторГромкости.Margin = new System.Windows.Forms.Padding(4);
            this.tBРегуляторГромкости.Maximum = 0;
            this.tBРегуляторГромкости.Minimum = -10000;
            this.tBРегуляторГромкости.Name = "tBРегуляторГромкости";
            this.tBРегуляторГромкости.Size = new System.Drawing.Size(407, 56);
            this.tBРегуляторГромкости.TabIndex = 0;
            this.tBРегуляторГромкости.Scroll += new System.EventHandler(this.tBРегуляторГромкости_Scroll);
            // 
            // pnСостояниеCIS
            // 
            this.pnСостояниеCIS.BackColor = System.Drawing.Color.Orange;
            this.pnСостояниеCIS.Controls.Add(this.lblСостояниеCIS);
            this.pnСостояниеCIS.Location = new System.Drawing.Point(292, 138);
            this.pnСостояниеCIS.Margin = new System.Windows.Forms.Padding(4);
            this.pnСостояниеCIS.Name = "pnСостояниеCIS";
            this.pnСостояниеCIS.Size = new System.Drawing.Size(279, 28);
            this.pnСостояниеCIS.TabIndex = 13;
            // 
            // lblСостояниеCIS
            // 
            this.lblСостояниеCIS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblСостояниеCIS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblСостояниеCIS.Location = new System.Drawing.Point(4, -1);
            this.lblСостояниеCIS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblСостояниеCIS.Name = "lblСостояниеCIS";
            this.lblСостояниеCIS.Size = new System.Drawing.Size(271, 30);
            this.lblСостояниеCIS.TabIndex = 12;
            this.lblСостояниеCIS.Text = "ЦИС НЕ на связи";
            this.lblСостояниеCIS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(5, 169);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1561, 23);
            this.progressBar.TabIndex = 16;
            // 
            // MainWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1572, 786);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.pnСостояниеCIS);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pnСостояние);
            this.Controls.Add(this.btnБлокировка);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView1);
            this.DoubleBuffered = true;
            this.Icon = global::MainExample.Properties.Resources.SmallIcon;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainWindowForm";
            this.Text = "Основное окно";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindowForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.pnСостояние.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBРегуляторГромкости)).EndInit();
            this.pnСостояниеCIS.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDayOfWeek;
        private System.Windows.Forms.Button btnБлокировка;
        private System.Windows.Forms.Label lblСостояние;
        private System.Windows.Forms.Panel pnСостояние;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnОбновитьСписок;
        private System.Windows.Forms.CheckBox cBАвтоматическаяГенерация;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnВоспроизвести;
        private System.Windows.Forms.TrackBar tBРегуляторГромкости;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Player_Label;
        private System.Windows.Forms.CheckBox cB_ВоспроизведениеДвиженияПоездов;
        private System.Windows.Forms.Panel pnСостояниеCIS;
        private System.Windows.Forms.Label lblСостояниеCIS;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}