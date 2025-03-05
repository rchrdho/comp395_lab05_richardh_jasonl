using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

// Author: Richard Ho, A01349477
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
            WEB = 1
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

        private void NewMenuItem_Clicked(object sender, EventArgs e)
        {

            NewImageDialog dialog = new NewImageDialog();
            dialog.Owner = this;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int height = dialog.GetHeight();
                int width = dialog.GetWidth();

                FormChild newMDIChild = new FormChild();
                // set the parent form of the child window
                newMDIChild.MdiParent = this;

                newMDIChild.SetDefaultImage(height, width);

                // display the new form 
                newMDIChild.Show();

                //this.isFileLoaded = true;
                //EnableDisableSaveAndSaveAs(isFileLoaded);
            }

        }


        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void OpenFromWebButton_Click(object sender, EventArgs e)
        {
            FormDialogWeb dlg = new FormDialogWeb();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string webURLPath = dlg.WebImageUrl;

                AddNewImageChildForm(webURLPath, FileSourceLocation.WEB);
            }
        }

        private void FileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (this.HasChildren)
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else
            {
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
            }
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }


        /// <summary>
        /// Helper method to get the image format based on the file extension.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private ImageFormat GetFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;  // Default to JPEG if no match
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChild child = (FormChild)this.ActiveMdiChild;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = child.Text;
            saveFileDialog1.Filter = "jpg|*.jpg|jpeg|*.jpeg|bmp|*.bmp|gif|*.gif";
            saveFileDialog1.Title = "Save Image As";
            ImageFormat format = ImageFormat.Jpeg;

            if (child.Text == "New Image")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        child.Image.Save(saveFileDialog1.FileName);
                        child.Text = saveFileDialog1.FileName;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, Text);
                        return;
                    }

                }
            }
            else {
                try
                {
                    string extension = Path.GetExtension(saveFileDialog1.FileName);
                    format = GetFormat(extension);

                    Image copiedImage = new Bitmap(child.Image.Width, child.Image.Height);
                    Graphics g = Graphics.FromImage(copiedImage);
                    g.DrawImage(child.Image, 0, 0);

                    child.Image.Dispose();
                    child.Image = copiedImage;
                    child.Image.Save(saveFileDialog1.FileName, format);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, Text);
                    return;
                }
            }
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            FormChild child = (FormChild)this.ActiveMdiChild;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = child.Text;
            saveFileDialog1.Filter = "jpg|.jpg|jpeg|.jpeg|bmp|.bmp|gif|.gif";
            saveFileDialog1.Title = "Save Image As";
            ImageFormat format = ImageFormat.Jpeg;


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    child.Image.Save(saveFileDialog1.FileName);
                    child.Text = saveFileDialog1.FileName;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, Text);
                    return;
                }
            }
        }
    }

}
