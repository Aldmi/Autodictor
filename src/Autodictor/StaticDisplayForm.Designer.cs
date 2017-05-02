﻿using System.Windows.Forms;

namespace MainExample
{
    partial class StaticDisplayForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cl_NumbOfTrain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl_numberOfPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl_Stations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl_note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btn_Show = new System.Windows.Forms.Button();
            this.lv_Devices = new System.Windows.Forms.ListView();
            this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl_NumbOfTrain,
            this.cl_numberOfPath,
            this.cl_Stations,
            this.cl_time,
            this.cl_note,
            this.Action});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(3, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1074, 579);
            this.dataGridView1.TabIndex = 0;
            // 
            // cl_NumbOfTrain
            // 
            this.cl_NumbOfTrain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cl_NumbOfTrain.HeaderText = "Номер поезда";
            this.cl_NumbOfTrain.Name = "cl_NumbOfTrain";
            this.cl_NumbOfTrain.Width = 116;
            // 
            // cl_numberOfPath
            // 
            this.cl_numberOfPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cl_numberOfPath.HeaderText = "Номер пути";
            this.cl_numberOfPath.Name = "cl_numberOfPath";
            this.cl_numberOfPath.Width = 101;
            // 
            // cl_Stations
            // 
            this.cl_Stations.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl_Stations.HeaderText = "Станция";
            this.cl_Stations.Name = "cl_Stations";
            // 
            // cl_time
            // 
            this.cl_time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cl_time.HeaderText = "Время";
            this.cl_time.Name = "cl_time";
            this.cl_time.Width = 74;
            // 
            // cl_note
            // 
            this.cl_note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cl_note.HeaderText = "Примечание";
            this.cl_note.Name = "cl_note";
            // 
            // Action
            // 
            this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Action.HeaderText = "действие";
            this.Action.Name = "Action";
            this.Action.Text = "Уд";
            this.Action.Width = 76;
            // 
            // btn_Show
            // 
            this.btn_Show.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Show.Location = new System.Drawing.Point(1083, 504);
            this.btn_Show.Name = "btn_Show";
            this.btn_Show.Size = new System.Drawing.Size(262, 72);
            this.btn_Show.TabIndex = 2;
            this.btn_Show.Text = "Отобразить";
            this.btn_Show.UseVisualStyleBackColor = true;
            this.btn_Show.Click += new System.EventHandler(this.btn_Show_Click);
            // 
            // lv_Devices
            // 
            this.lv_Devices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lv_Devices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id,
            this.Name});
            this.lv_Devices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lv_Devices.Location = new System.Drawing.Point(1083, -2);
            this.lv_Devices.Name = "lv_Devices";
            this.lv_Devices.Size = new System.Drawing.Size(262, 495);
            this.lv_Devices.TabIndex = 3;
            this.lv_Devices.UseCompatibleStateImageBehavior = false;
            this.lv_Devices.View = System.Windows.Forms.View.Details;
            // 
            // Id
            // 
            this.Id.Text = "Id";
            // 
            // Name
            // 
            this.Name.Text = "Название";
            this.Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Name.Width = 200;
            // 
            // StaticDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(600, 400);
            this.ClientSize = new System.Drawing.Size(1345, 583);
            this.Controls.Add(this.lv_Devices);
            this.Controls.Add(this.btn_Show);
            this.Controls.Add(this.dataGridView1);
            //this.Name = "StaticDisplayForm";
            this.Text = "Отображение статической информации";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_Show;
        private System.Windows.Forms.ListView lv_Devices;
        private System.Windows.Forms.ColumnHeader Id;
        private System.Windows.Forms.ColumnHeader Name;
        private DataGridViewTextBoxColumn cl_NumbOfTrain;
        private DataGridViewTextBoxColumn cl_numberOfPath;
        private DataGridViewTextBoxColumn cl_Stations;
        private DataGridViewTextBoxColumn cl_time;
        private DataGridViewTextBoxColumn cl_note;
        private DataGridViewButtonColumn Action;
    }
}