using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Forms;
using System.Diagnostics;

namespace WinFormsApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var child = new Form();

            child.MdiParent = this;

            child.Show();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                }
            }
        }

        private void openFromWebButton_Click(object sender, EventArgs e)
        {
            FormDialogWeb dlg = new FormDialogWeb();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Form1 child = new Form1();
                child.MdiParent = this;
                string strURL = dlg.WebImageUrl;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
