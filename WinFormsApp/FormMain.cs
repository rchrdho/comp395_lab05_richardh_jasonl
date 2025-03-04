using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

// Author: Richard Ho, 
// Author: Jason Lau, A01343986
// Last Modified: 2025-03-03

namespace WinFormsApp
{

    //TODO: add OnClose type event to remove of the ImageForm child component from the main form, and call Dispose() on it and its Image field

    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public enum FileSourceLocation
        {
            FILE_SYSTEM = 0,
            WEB         = 1
        }

        public void AddNewImageChildForm(string imageFilePath, FileSourceLocation fileSourceLocation)
        {
            ImageForm imageChildForm = new ImageForm();
            imageChildForm.MdiParent = this;

            // Logic for loading the image

            // ***Not good OOP practice (should instead use open/closed principle and abstract base class) but we won't be adding any more file source locations anyway
            if (fileSourceLocation == FileSourceLocation.FILE_SYSTEM)
            {
                //TODO - use files / streams to get the image from user's file system
            }
            else if (fileSourceLocation == FileSourceLocation.WEB)
            {
                //TODO - use WebRequest and WebResponse for loading from the Web
            }

            //TODO - set the Image field in the image child form
            //imageChildForm.Image = 

            imageChildForm.Show(); //TODO - remove this, and instead trigger the ImageForm FormChild_Paint event handler (idk how to do this yet)
        }

        // Event handlers ===

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var dialog = new NewImageDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Size chosenSize = dialog.SelectedSize;


                    var child = new ImageForm();
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

                    AddNewImageChildForm(filePath, FileSourceLocation.FILE_SYSTEM);
                }
            }
        }

        private void openFromWebButton_Click(object sender, EventArgs e)
        {
            FormDialogWeb dlg = new FormDialogWeb();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string webURLPath = dlg.WebImageUrl;

                AddNewImageChildForm(webURLPath, FileSourceLocation.WEB);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////save the file
            //FormChild child = (FormChild)this.ActiveMdiChild;
            //this.saveFileDialog1.FileName = child.Text;
            //this.saveFileDialog1.Filter = "jpg|*.jpg|jpeg|*.jpeg|bmp|*.bmp|gif|*.gif";
            //this.saveFileDialog1.Title = "Save Image As";
            //ImageFormat format = ImageFormat.Jpeg;
            //if (child.Text == "New Image")
            //{
            //    //save the file dilaog - new child
            //    if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        try
            //        {
            //            child.Image.Save(this.saveFileDialog1.FileName);
            //            child.Text = this.saveFileDialog1.FileName;
            //        }
            //        catch (Exception exc)
            //        {
            //            MessageBox.Show(exc.Message, Text);
            //            return;
            //        }
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        // get the extension of the file
            //        string extension = Path.GetExtension(saveFileDialog1.FileName);
            //        format = GetFormat(extension);
            //        //copy original
            //        Image copyImage = new
            //        Bitmap(child.Image.Width, child.Image.Height);
            //        //get graphics
            //        Graphics g = Graphics.FromImage(copyImage);
            //        //draw the image
            //        g.DrawImage(child.Image, 0, 0);
            //        //close original
            //        child.Image.Dispose();
            //        child.Image = copyImage;
            //        child.Image.Save(this.saveFileDialog1.FileName, format);
            //    }
            //    catch (Exception exc)
            //    {
            //        MessageBox.Show(exc.Message, "My Error");
            //        return;
            //    }
            //}
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (this.HasChildren)
            {
                saveToolStripMenuItem1.Enabled  = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else
            {
                saveToolStripMenuItem1.Enabled  = false;
                saveAsToolStripMenuItem.Enabled = false;
            }
        }
    }
}
