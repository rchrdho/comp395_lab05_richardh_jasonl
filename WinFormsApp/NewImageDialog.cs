using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class NewImageDialog : Form
    {
        public Size SelectedSize { get; set; }

        public NewImageDialog()
        {
            InitializeComponent();
        }
        
        private void NewImageDialog_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SelectedSize = new Size(640, 480);

            }
            else if (radioButton2.Checked)
            {
                SelectedSize = new Size(800, 600);

            }
            else if (radioButton3.Checked)
            {
                SelectedSize = new Size(1024, 768);
            }
            else
            {
                SelectedSize = new Size(640, 480);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
