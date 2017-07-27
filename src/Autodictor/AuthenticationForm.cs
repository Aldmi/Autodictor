using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainExample
{
    public partial class AuthenticationForm : Form
    {
        public bool IsAuthentication { get; set; }



        public AuthenticationForm()
        {
            InitializeComponent();
            CreateMyPasswordTextBox();
        }



        public void CreateMyPasswordTextBox()
        {
            // Set the maximum length of text in the control to eight.
            tb_password.MaxLength = 8;
            // Assign the asterisk to be the password character.
            tb_password.PasswordChar = '*';
            // Change all text entered to be lowercase.
            tb_password.CharacterCasing = CharacterCasing.Lower;
            // Align the text in the center of the TextBox control.
            tb_password.TextAlign = HorizontalAlignment.Center;
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            IsAuthentication = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();       
        }
    }
}
