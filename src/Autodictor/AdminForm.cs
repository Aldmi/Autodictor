using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Entitys.Authentication;

namespace MainExample
{
    public partial class AdminForm : Form
    {
        public List<User> Users { get; set; }



        public AdminForm()
        {
            InitializeComponent();

            Users= Program.UsersDbRepository.List().ToList();
            FillTable(Users);
        }





        private void FillTable(IEnumerable<User> users )
        {
            if (users == null || !users.Any())
            {
                return;
            }


            foreach (var user in users)
            {
                this.dgv_пользователи.Rows.Add(new object[] { "col1", "col2"  });
            }


            var column = dgv_пользователи.Columns[2] as DataGridViewComboBoxColumn;
            if (column != null)
            {
                column.DataSource = Enum.GetValues(typeof(Role));
            }

            foreach (DataGridViewRow row in dgv_пользователи.Rows)
            {
                DataGridViewComboBoxCell cell = row.Cells[2] as DataGridViewComboBoxCell;
                if (cell != null)
                {
                    cell.Value = Role.Наблюдатель; //cell.Items[2];
                }
            }
        }




        /// <summary>
        /// Нумерация строк
        /// </summary>
        private void dgv_RowPrePaint(object sender,DataGridViewRowPrePaintEventArgs e)
        {
            object head = this.dgv_пользователи.Rows[e.RowIndex].HeaderCell.Value;
            if (head == null || !head.Equals((e.RowIndex + 1).ToString()))
                this.dgv_пользователи.Rows[e.RowIndex].HeaderCell.Value =
                    (e.RowIndex + 1).ToString();
        }




    }
}
