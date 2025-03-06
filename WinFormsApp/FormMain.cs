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
        /// <summary>
        /// HttpClient private instance variable, used to make http requests. 
        /// Intended to be instantiated once per application, rather than per-use.
        /// </summary>
        private static readonly HttpClient httpClient = new HttpClient();

        public FormMain()
        {
            InitializeComponent();
        }

        private enum FileSourceLocation
        {
            FILE_SYSTEM = 0,
            WEB = 1
        }

        /// <summary>
        /// Creates an Image subclass instance from a byte array representation of an image.
        /// </summary>
        /// <param name="bytesArrOfImg"></param>
        /// <returns></returns>
        private static Image CreateImageFromByteArray(byte[] bytesArrOfImg)
        {
            // Write the image data (in form of bytes array) to a memory stream
            MemoryStream mStream = new MemoryStream();
            mStream.Write(bytesArrOfImg, 0, bytesArrOfImg.Length);

            // Create the image using the created memory stream
            Image image = Image.FromStream(mStream);

            return image;
        }

        //TODO: add try and exception handling for both image loading methods

        /// <summary>
        /// Loads and returns an image from the user's file system.
        /// </summary>
        /// <param name="imageFSSourcePath"></param>
        /// <returns></returns>
        private static async Task<Image> GetFileSystemImage(string imageFSSourcePath)
        {
            Image image;
            byte[] bytesArrOfImg;

            using (FileStream sourceStream = File.Open(imageFSSourcePath, FileMode.Open))
            {
                bytesArrOfImg = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(bytesArrOfImg, 0, (int)sourceStream.Length);
            }

            image = CreateImageFromByteArray(bytesArrOfImg);
            return image;
        }

        /// <summary>
        /// Loads and returns an image from the web using HttpClient, passing in the uri of the source location.
        /// HttpClient does not need to be disposed after every request, it should exist during the entire application runtime.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="reqURI"></param>
        /// <returns></returns>
        private static async Task<Image> GetWebImage(HttpClient httpClient, string imageSrcURI)
        {
            Image image;
            byte[] bytesArrOfImg;

            // Make GET request and receive response (the image in bytes)
            bytesArrOfImg = await httpClient.GetByteArrayAsync(imageSrcURI);

            image = CreateImageFromByteArray(bytesArrOfImg);
            return image;
        }

        /// <summary>
        /// Adds a new image (child form) to the main form "canvas" after retrieving it using the image file path / url.
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <param name="fileSourceLocation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private async void AddNewImageChildForm(string imageFilePath, FileSourceLocation fileSourceLocation)
        {
            ImageForm imageChildForm = new ImageForm();
            Image image;
            imageChildForm.MdiParent = this;

            if (imageFilePath == null || String.IsNullOrWhiteSpace(imageFilePath))
            {
                throw new ArgumentNullException($"File path or web url path cannot be null, empty, or only contain whitespace.");
            }

            // Load the image from the user's file system or from the web

            // ***Not good OOP practice (should instead use open/closed principle and abstract base class) but we won't be adding any more file source locations anyway
            if (fileSourceLocation == FileSourceLocation.FILE_SYSTEM)
            {
                image = await GetFileSystemImage(imageFilePath);
                imageChildForm.Image = image;
            }
            else if (fileSourceLocation == FileSourceLocation.WEB)
            {
                image = await GetWebImage(httpClient, imageFilePath);
                imageChildForm.Image = image;
            }

            // reload all events in the ImageForm child component
            // (will trigger the Paint event handler that draws the image to the child form)
            imageChildForm.Invalidate();

            imageChildForm.Show();
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
            if (this.MdiChildren.Length > 0)
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
            else
            {
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length > 0) {
                // create a new child form - FormMainCloseDialog
                MessageBox.Show("Save?"); //temp
            }
            this.Close();
            Application.Exit();
        }
    }

}
