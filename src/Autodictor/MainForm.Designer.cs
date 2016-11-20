﻿namespace MainExample
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.controlSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.trainMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.regularMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.alarmMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.staticSound = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamicSound = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просмотрСправкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.таблоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Boards = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеЦИСToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OperativeShedules = new System.Windows.Forms.ToolStripMenuItem();
            this.RegulatoryShedules = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.controlSamplesToolStripMenuItem,
            this.dataSamplesToolStripMenuItem,
            this.справкаToolStripMenuItem,
            this.таблоToolStripMenuItem,
            this.данныеЦИСToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(1344, 28);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.toolStripMenuItem1,
            this.fileExit});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(57, 24);
            this.fileMenu.Text = "&Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("открытьToolStripMenuItem.Image")));
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.открытьToolStripMenuItem.Text = "&Открыть...";
            this.открытьToolStripMenuItem.ToolTipText = "Открытие конфигурации, из текстового файла";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("сохранитьToolStripMenuItem.Image")));
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.сохранитьToolStripMenuItem.Text = "&Сохранить...";
            this.сохранитьToolStripMenuItem.ToolTipText = "Сохранение конфигурации в текстовом файле";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(214, 6);
            // 
            // fileExit
            // 
            this.fileExit.Name = "fileExit";
            this.fileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.fileExit.Size = new System.Drawing.Size(217, 26);
            this.fileExit.Text = "В&ыход";
            this.fileExit.ToolTipText = "Завершение программы";
            this.fileExit.Click += new System.EventHandler(this.fileExit_Click);
            // 
            // controlSamplesToolStripMenuItem
            // 
            this.controlSamplesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainWindow,
            this.trainMessages,
            this.regularMessages,
            this.alarmMessages});
            this.controlSamplesToolStripMenuItem.Name = "controlSamplesToolStripMenuItem";
            this.controlSamplesToolStripMenuItem.Size = new System.Drawing.Size(103, 24);
            this.controlSamplesToolStripMenuItem.Text = "&Расписание";
            // 
            // mainWindow
            // 
            this.mainWindow.Name = "mainWindow";
            this.mainWindow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.mainWindow.Size = new System.Drawing.Size(359, 26);
            this.mainWindow.Text = "&Основное окно";
            this.mainWindow.ToolTipText = "Окно, содержащее текущий список программы";
            this.mainWindow.Click += new System.EventHandler(this.buttonExample_Click);
            // 
            // trainMessages
            // 
            this.trainMessages.Name = "trainMessages";
            this.trainMessages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F2)));
            this.trainMessages.Size = new System.Drawing.Size(359, 26);
            this.trainMessages.Text = "&Расписание движения поездов";
            this.trainMessages.Click += new System.EventHandler(this.listExample_Click);
            // 
            // regularMessages
            // 
            this.regularMessages.Name = "regularMessages";
            this.regularMessages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
            this.regularMessages.Size = new System.Drawing.Size(359, 26);
            this.regularMessages.Text = "Р&егулярные сообщения";
            this.regularMessages.Click += new System.EventHandler(this.validationExample_Click);
            // 
            // alarmMessages
            // 
            this.alarmMessages.Name = "alarmMessages";
            this.alarmMessages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.alarmMessages.Size = new System.Drawing.Size(359, 26);
            this.alarmMessages.Text = "&Внештатные сообщения";
            this.alarmMessages.Click += new System.EventHandler(this.textBoxExample_Click);
            // 
            // dataSamplesToolStripMenuItem
            // 
            this.dataSamplesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staticSound,
            this.dynamicSound});
            this.dataSamplesToolStripMenuItem.Name = "dataSamplesToolStripMenuItem";
            this.dataSamplesToolStripMenuItem.Size = new System.Drawing.Size(171, 24);
            this.dataSamplesToolStripMenuItem.Text = "&Звуковые сообщения";
            // 
            // staticSound
            // 
            this.staticSound.Name = "staticSound";
            this.staticSound.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
            this.staticSound.Size = new System.Drawing.Size(324, 26);
            this.staticSound.Text = "&Статические сообщения";
            this.staticSound.ToolTipText = "Список статических сообщений системы";
            this.staticSound.Click += new System.EventHandler(this.dataSetExample_Click);
            // 
            // dynamicSound
            // 
            this.dynamicSound.Name = "dynamicSound";
            this.dynamicSound.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F2)));
            this.dynamicSound.Size = new System.Drawing.Size(324, 26);
            this.dynamicSound.Text = "&Динамические сообщения";
            this.dynamicSound.Click += new System.EventHandler(this.arrayDataSourceExample_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.просмотрСправкиToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(79, 24);
            this.справкаToolStripMenuItem.Text = "&Справка";
            // 
            // просмотрСправкиToolStripMenuItem
            // 
            this.просмотрСправкиToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("просмотрСправкиToolStripMenuItem.Image")));
            this.просмотрСправкиToolStripMenuItem.Name = "просмотрСправкиToolStripMenuItem";
            this.просмотрСправкиToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.просмотрСправкиToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.просмотрСправкиToolStripMenuItem.Text = "&Просмотр справки";
            this.просмотрСправкиToolStripMenuItem.Click += new System.EventHandler(this.просмотрСправкиToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("оПрограммеToolStripMenuItem.Image")));
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.оПрограммеToolStripMenuItem.Text = "&О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // таблоToolStripMenuItem
            // 
            this.таблоToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Boards});
            this.таблоToolStripMenuItem.Name = "таблоToolStripMenuItem";
            this.таблоToolStripMenuItem.Size = new System.Drawing.Size(99, 24);
            this.таблоToolStripMenuItem.Text = "Устройства";
            // 
            // Boards
            // 
            this.Boards.Name = "Boards";
            this.Boards.Size = new System.Drawing.Size(126, 26);
            this.Boards.Text = "Табло";
            this.Boards.Click += new System.EventHandler(this.Boards_Click);
            // 
            // данныеЦИСToolStripMenuItem
            // 
            this.данныеЦИСToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OperativeShedules,
            this.RegulatoryShedules});
            this.данныеЦИСToolStripMenuItem.Name = "данныеЦИСToolStripMenuItem";
            this.данныеЦИСToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.данныеЦИСToolStripMenuItem.Text = "Данные ЦИС";
            // 
            // OperativeShedules
            // 
            this.OperativeShedules.Name = "OperativeShedules";
            this.OperativeShedules.Size = new System.Drawing.Size(265, 26);
            this.OperativeShedules.Text = "Оперативное расписание";
            this.OperativeShedules.Click += new System.EventHandler(this.OperativeShedules_Click);
            // 
            // RegulatoryShedules
            // 
            this.RegulatoryShedules.Name = "RegulatoryShedules";
            this.RegulatoryShedules.Size = new System.Drawing.Size(265, 26);
            this.RegulatoryShedules.Text = "Регулярное расписание";
            this.RegulatoryShedules.Click += new System.EventHandler(this.RegulatoryShedules_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 898);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.Icon = global::MainExample.Properties.Resources.SmallIcon;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Автодиктор - программа автоматического информирования пассажиров";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem fileExit;
        private System.Windows.Forms.ToolStripMenuItem controlSamplesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataSamplesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainWindow;
        private System.Windows.Forms.ToolStripMenuItem trainMessages;
        private System.Windows.Forms.ToolStripMenuItem regularMessages;
        private System.Windows.Forms.ToolStripMenuItem alarmMessages;
        private System.Windows.Forms.ToolStripMenuItem staticSound;
        private System.Windows.Forms.ToolStripMenuItem dynamicSound;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просмотрСправкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem данныеЦИСToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OperativeShedules;
        private System.Windows.Forms.ToolStripMenuItem таблоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Boards;
        private System.Windows.Forms.ToolStripMenuItem RegulatoryShedules;
    }
}

