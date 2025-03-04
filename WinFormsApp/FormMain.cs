namespace WinFormsApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var child = new Form();

            child.MdiParent = this;

            child.Show();
        }
    }
}
