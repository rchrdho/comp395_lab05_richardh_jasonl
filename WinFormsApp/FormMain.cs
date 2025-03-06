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
    
    /// <summary>
    /// Represents the main MDI (Multiple Document Interface) form of the application.
    /// It hosts and manages multiple image child forms and provides functionality
    /// for opening, saving, tiling, and cascading these image forms.
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// HttpClient private instance variable, used to make http requests. 
        /// Intended to be instantiated once per application, rather than per-use.
        /// </summary>
        private static readonly HttpClient httpClient = new HttpClient();
       
        /// <summary>
        /// Default constructor for the FormMain class. Initializes the main form's components.
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Specifies the file source location types: either from the user's file system or from the web.
        /// </summary>
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
        private static async Task<Image?> GetFileSystemImage(string imageFSSourcePath)
        {
            Image image;
            byte[] bytesArrOfImg;

            try
            {
                using (FileStream sourceStream = File.Open(imageFSSourcePath, FileMode.Open))
                {
                    bytesArrOfImg = new byte[sourceStream.Length];
                    await sourceStream.ReadAsync(bytesArrOfImg, 0, (int)sourceStream.Length);
                }

                image = CreateImageFromByteArray(bytesArrOfImg);
                return image;
            }
            catch(FileNotFoundException ex)
            {
                // Handle file not found exception
                MessageBox.Show($"File not found: {ex.Message}");
                return null;
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle access permissions issues
                MessageBox.Show($"Access denied: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                MessageBox.Show($"An unknown error occurred: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Loads and returns an image from the web using HttpClient, passing in the uri of the source location.
        /// HttpClient does not need to be disposed after every request, it should exist during the entire application runtime.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="reqURI"></param>
        /// <returns></returns>
        private static async Task<Image?> GetWebImage(HttpClient httpClient, string imageSrcURI)
        {
            Image image;
            byte[] bytesArrOfImg;

            try
            {
                // Make GET request and receive response (the image in bytes)
                bytesArrOfImg = await httpClient.GetByteArrayAsync(imageSrcURI);
                image = CreateImageFromByteArray(bytesArrOfImg);
                return image;
            }
            catch(UriFormatException)
            {
                MessageBox.Show("Invalid uri. The format of the uri could not be determined.");
                return null;
            }
            catch(HttpRequestException ex)
            {
                MessageBox.Show($"Unable to load image. An HTTP request error occured. Error msg: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unknown Exception occured. Exception msg: {ex.Message}");
                return null;
            }            
        }

        /// <summary>
        /// Adds a new image (child form) to the main form "canvas" after retrieving it using the image file path / url.
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <param name="fileSourceLocation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private async void AddNewImageChildForm(string imageFilePath, FileSourceLocation fileSourceLocation)
        {
            ImageFormChild imageChildForm = new ImageFormChild();
            Image? image;
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
                if (image != null) imageChildForm.Image = image;
            }
            else if (fileSourceLocation == FileSourceLocation.WEB)
            {
                image = await GetWebImage(httpClient, imageFilePath);
                if (image != null) imageChildForm.Image = image;
            }

            // reload all events in the ImageForm child component
            // (will trigger the Paint event handler that draws the image to the child form)
            imageChildForm.Invalidate();

            imageChildForm.Show();
        }

        // Event handlers ===

        /// <summary>
        /// Event handler for the "New" menu item click event. Displays a dialog for creating a new image
        /// with specified dimensions, creates the image, and shows it in a new child form.
        /// </summary>
        private void NewMenuItem_Clicked(object sender, EventArgs e)
        {

            NewImageDialog dialog = new NewImageDialog();
            dialog.Owner          = this;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int height = dialog.GetHeight();
                int width  = dialog.GetWidth();

                ImageFormChild newMDIChild = new ImageFormChild();
                // set the parent form of the child window
                newMDIChild.MdiParent      = this;

                newMDIChild.SetDefaultImage(height, width);

                // display the new form 
                newMDIChild.Show();
            }
        }

        /// <summary>
        /// Event handler for opening an image from the user's file system via the OpenFileDialog.
        /// </summary>
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

        /// <summary>
        /// Event handler for loading an image from the web, prompting the user for a URL,
        /// and creating a new child form to display the downloaded image.
        /// </summary>
        private void OpenFromWebButton_Click(object sender, EventArgs e)
        {
            FormDialogWeb dlg = new FormDialogWeb();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string webURLPath = dlg.WebImageUrl;

                AddNewImageChildForm(webURLPath, FileSourceLocation.WEB);
            }
            dlg.Dispose();
        }

        /// <summary>
        /// Event handler for the File menu's DropDownOpening event. Enables or disables
        /// the Save and Save As menu items depending on whether any MDI children exist.
        /// </summary>
        private void FileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length > 0)
            {
                saveToolStripMenuItem.Enabled   = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else
            {
                saveToolStripMenuItem.Enabled   = false;
                saveAsToolStripMenuItem.Enabled = false;
            }

        }


        /// <summary>
        /// Event handler for cascading all open MDI child windows.
        /// </summary>
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        /// <summary>
        /// Event handler for arranging all open MDI child windows horizontally.
        /// </summary>
        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        /// <summary>
        /// Event handler for arranging all open MDI child windows vertically.
        /// </summary>
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

        /// <summary>
        /// Event handler for saving the active MDI child image.
        /// Displays a SaveFileDialog if the image is new, or attempts to save it directly
        /// if it has an existing file name. Allows for format conversion based on extension.
        /// </summary>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageFormChild child           = (ImageFormChild)this.ActiveMdiChild;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName       = child.Text;
            saveFileDialog1.Filter         = "jpg|*.jpg|jpeg|*.jpeg|bmp|*.bmp|gif|*.gif";
            saveFileDialog1.Title          = "Save Image";
            ImageFormat format             = ImageFormat.Jpeg;

            // Check if current form is "new image"
            if (child.Text == "New Image")
            {
                // if Dialog box button Ok is clicked
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
                    // if file is named smt else or just not new image copy image to bitmap and use graphics to draw in
                    string extension = Path.GetExtension(saveFileDialog1.FileName);
                    format = GetFormat(extension);

                    Image copiedImage = new Bitmap(child.Image.Width, child.Image.Height);
                    Graphics g = Graphics.FromImage(copiedImage);
                    g.DrawImage(child.Image, 0, 0);

                    child.Image.Dispose();
                    child.Image = copiedImage;
                    // save to properly formatted image (as long as getformat accepts that type)
                    child.Image.Save(saveFileDialog1.FileName, format);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, Text);
                    return;
                }
            }
        }

        /// <summary>
        /// Event handler for the "Save As" menu item. Prompts the user for a save location
        /// and file name, then saves the currently active MDI child image to the chosen format.
        /// </summary>
        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            ImageFormChild child     = (ImageFormChild)this.ActiveMdiChild;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter   = "jpg|.jpg|jpeg|.jpeg|bmp|.bmp|gif|.gif";
            saveFileDialog1.Title    = "Save Image As";
            ImageFormat format       = ImageFormat.Jpeg;

            // The same as Save image menu but does not check for a default name. Must input name.
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

        /// <summary>
        /// Event handler for the "Exit" menu item. If there are any open MDI child forms, prompts the user
        /// with a dialog that allows saving before closing. Otherwise, closes the application immediately.
        /// </summary>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length > 0)
            {
                FormMainCloseDialog closeDialogForm = new FormMainCloseDialog();

                if (closeDialogForm.ShowDialog() == DialogResult.OK)
                {
                    SaveToolStripMenuItem_Click(sender, e);
                }
                closeDialogForm.Dispose();
            }
            this.Close();
            this.Dispose();
            Application.Exit();
        }
    }
}
