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
    /// Child form
    /// </summary>
    public partial class ImageFormChild : Form
    {
        public ImageFormChild()
        {
            InitializeComponent();
            SetDefaultImage(300, 300);
        }

        // field Image
        private Image myImage;

        /// <summary>
        /// property Image
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
        /// Paint event handler for child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChild_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(myImage, this.AutoScrollPosition.X,
            this.AutoScrollPosition.Y, myImage.Width,
            myImage.Height);
        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            myImage.Dispose();
            this.Dispose();
        }
    }
}