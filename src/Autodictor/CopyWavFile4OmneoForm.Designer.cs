namespace MainExample
{
    partial class CopyWavFile4OmneoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyWavFile4OmneoForm));
            this.btn_Convert = new System.Windows.Forms.Button();
            this.btn_Source = new System.Windows.Forms.Button();
            this.btn_Dest = new System.Windows.Forms.Button();
            this.tb_PathSource = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tb_PathDest = new System.Windows.Forms.TextBox();
            this.prBar = new System.Windows.Forms.ProgressBar();
            this.tb_Statys = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Convert
            // 
            this.btn_Convert.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Convert.Location = new System.Drawing.Point(10, 201);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(170, 86);
            this.btn_Convert.TabIndex = 9;
            this.btn_Convert.Text = "Конвертировать";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Convert_Click);
            // 
            // btn_Source
            // 
            this.btn_Source.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Source.Location = new System.Drawing.Point(6, 25);
            this.btn_Source.Name = "btn_Source";
            this.btn_Source.Size = new System.Drawing.Size(97, 30);
            this.btn_Source.TabIndex = 0;
            this.btn_Source.Text = "Источник";
            this.btn_Source.UseVisualStyleBackColor = true;
            this.btn_Source.Click += new System.EventHandler(this.btn_Source_Click);
            // 
            // btn_Dest
            // 
            this.btn_Dest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Dest.Location = new System.Drawing.Point(6, 61);
            this.btn_Dest.Name = "btn_Dest";
            this.btn_Dest.Size = new System.Drawing.Size(97, 30);
            this.btn_Dest.TabIndex = 1;
            this.btn_Dest.Text = "Результат";
            this.btn_Dest.UseVisualStyleBackColor = true;
            this.btn_Dest.Click += new System.EventHandler(this.btn_Dest_Click);
            // 
            // tb_PathSource
            // 
            this.tb_PathSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_PathSource.Location = new System.Drawing.Point(121, 28);
            this.tb_PathSource.Name = "tb_PathSource";
            this.tb_PathSource.ReadOnly = true;
            this.tb_PathSource.Size = new System.Drawing.Size(596, 26);
            this.tb_PathSource.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.tb_PathDest);
            this.groupBox1.Controls.Add(this.btn_Source);
            this.groupBox1.Controls.Add(this.btn_Dest);
            this.groupBox1.Controls.Add(this.tb_PathSource);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(725, 192);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Директории";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(7, 95);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(710, 89);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // tb_PathDest
            // 
            this.tb_PathDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_PathDest.Location = new System.Drawing.Point(121, 63);
            this.tb_PathDest.Name = "tb_PathDest";
            this.tb_PathDest.ReadOnly = true;
            this.tb_PathDest.Size = new System.Drawing.Size(596, 26);
            this.tb_PathDest.TabIndex = 4;
            // 
            // prBar
            // 
            this.prBar.Location = new System.Drawing.Point(186, 290);
            this.prBar.Name = "prBar";
            this.prBar.Size = new System.Drawing.Size(540, 36);
            this.prBar.Step = 1;
            this.prBar.TabIndex = 11;
            // 
            // tb_Statys
            // 
            this.tb_Statys.Location = new System.Drawing.Point(186, 198);
            this.tb_Statys.Multiline = true;
            this.tb_Statys.Name = "tb_Statys";
            this.tb_Statys.ReadOnly = true;
            this.tb_Statys.Size = new System.Drawing.Size(540, 89);
            this.tb_Statys.TabIndex = 10;
            // 
            // CopyWavFile4OmneoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 333);
            this.Controls.Add(this.btn_Convert);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.prBar);
            this.Controls.Add(this.tb_Statys);
            this.Name = "CopyWavFile4OmneoForm";
            this.Text = "Окно конвертирования звуковых файлов";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.Button btn_Source;
        private System.Windows.Forms.Button btn_Dest;
        private System.Windows.Forms.TextBox tb_PathSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_PathDest;
        private System.Windows.Forms.ProgressBar prBar;
        private System.Windows.Forms.TextBox tb_Statys;
        private System.Windows.Forms.TextBox textBox3;
    }
}