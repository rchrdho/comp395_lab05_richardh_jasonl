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
    public partial class FormDialogWeb : Form
    {
        public FormDialogWeb()
        {
            InitializeComponent();
            WebImageUrl = "https://www.w3schools.com/css/img_5terre.jpg"; // Default url
        }

        public string WebImageUrl { get; set; }

        private void WebDialogOk_Button_Click(object sender, EventArgs e)
        {
            WebImageUrl = txtURL.Text.Trim();

            if (string.IsNullOrEmpty(WebImageUrl))
            {
                MessageBox.Show("Please enter a valid URL.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void WebDialogCancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
