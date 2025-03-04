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

        private void ToolStripMenuNew_Click(object sender, EventArgs e)
        {
            using (var dialog = new NewImageDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Size chosenSize = dialog.SelectedSize;


                    var child = new Form1();
                    child.MdiParent = this;
                    child.ClientSize = chosenSize;
                    child.BackColor = Color.LightBlue;
                    child.Show();
                }
            }
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
    }
}
