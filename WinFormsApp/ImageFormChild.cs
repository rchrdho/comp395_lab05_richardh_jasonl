using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp
{
    /// <summary>
    /// Represents a child form in the MDI application,
    /// containing and displaying an image.
    /// </summary>
    public partial class ImageFormChild : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFormChild"/> class
        /// and sets a default image of 300x300 filled with a light blue color.
        /// </summary>
        public ImageFormChild()
        {
            InitializeComponent();
            SetDefaultImage(300, 300);
        }

        // field Image
        private Image myImage;

        /// <summary>
        /// Gets or sets the <see cref="Image"/> displayed in this child form.
        /// When set, the <see cref="AutoScrollMinSize"/> property is adjusted
        /// to match the size of the provided image.
        /// </summary>
        public Image Image
        {
            set
            {
                myImage = value;
                this.AutoScrollMinSize = myImage.Size;
            }
            get
            {
                return myImage;
            }
        }

        /// <summary>
        /// Sets the default image with specified <paramref name="height"/> and <paramref name="width"/>,
        /// then fills it with a light blue color.
        /// </summary>
        /// <param name="height">The height of the default image.</param>
        /// <param name="width">The width of the default image.</param>
        public void SetDefaultImage(int height, int width)
        {
            Image = new Bitmap(height, width);

            using (Graphics g = Graphics.FromImage(Image))
            using (SolidBrush brush = new SolidBrush(Color.LightBlue))
            {
                g.FillRectangle(brush, 0, 0, height, width);
            }
        }

        /// <summary>
        /// Handles the Paint event of the form. Draws the current <see cref="myImage"/>
        /// onto the form, taking the auto-scroll position into account.
        /// </summary>
        /// <param name="sender">The source of the event (not used).</param>
        /// <param name="e">Provides data for the Paint event.</param>
        private void FormChild_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(myImage, this.AutoScrollPosition.X,
            this.AutoScrollPosition.Y, myImage.Width,
            myImage.Height);
        }

        /// <summary>
        /// Handles the FormClosed event of the child form. Disposes of the
        /// underlying image and the form resources when the form is closed.
        /// </summary>
        /// <param name="sender">The source of the event (not used).</param>
        /// <param name="e">Provides data for the FormClosed event.</param>
        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            myImage.Dispose();
            this.Dispose();
        }
    }
}
