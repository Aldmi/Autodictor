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
    public partial class TranslateForm : Form
    {
        public TranslateForm(IEnumerable<string> translateWords)
        {
            InitializeComponent();


            foreach (var t in translateWords)
            {
                GVtranslate.Rows.Add(t);
            }
        }
    }
}
