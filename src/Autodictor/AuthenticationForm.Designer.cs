namespace MainExample
{
    partial class AuthenticationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Roles = new System.Windows.Forms.ComboBox();
            this.cb_Users = new System.Windows.Forms.ComboBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.btn_Enter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(34, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Роль";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(34, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пользователь";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(34, 151);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 38);
            this.label3.TabIndex = 2;
            this.label3.Text = "Пароль";
            // 
            // cb_Roles
            // 
            this.cb_Roles.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_Roles.FormattingEnabled = true;
            this.cb_Roles.Location = new System.Drawing.Point(250, 20);
            this.cb_Roles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_Roles.Name = "cb_Roles";
            this.cb_Roles.Size = new System.Drawing.Size(254, 28);
            this.cb_Roles.TabIndex = 3;
            // 
            // cb_Users
            // 
            this.cb_Users.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_Users.FormattingEnabled = true;
            this.cb_Users.Location = new System.Drawing.Point(250, 85);
            this.cb_Users.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cb_Users.Name = "cb_Users";
            this.cb_Users.Size = new System.Drawing.Size(254, 28);
            this.cb_Users.TabIndex = 4;
            // 
            // tb_password
            // 
            this.tb_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_password.Location = new System.Drawing.Point(250, 151);
            this.tb_password.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(254, 26);
            this.tb_password.TabIndex = 5;
            // 
            // btn_Enter
            // 
            this.btn_Enter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Enter.Location = new System.Drawing.Point(248, 230);
            this.btn_Enter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Enter.Name = "btn_Enter";
            this.btn_Enter.Size = new System.Drawing.Size(256, 82);
            this.btn_Enter.TabIndex = 6;
            this.btn_Enter.Text = "Войти";
            this.btn_Enter.UseVisualStyleBackColor = true;
            this.btn_Enter.Click += new System.EventHandler(this.btn_Enter_Click);
            // 
            // AuthenticationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 324);
            this.Controls.Add(this.btn_Enter);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.cb_Users);
            this.Controls.Add(this.cb_Roles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AuthenticationForm";
            this.Text = "Форма входа пользователя";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_Roles;
        private System.Windows.Forms.ComboBox cb_Users;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Button btn_Enter;
    }
}